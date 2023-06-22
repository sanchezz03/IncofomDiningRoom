using Dapper;
using InfocomDiningRoom.Core.Models.Menu;
using InfocomDinnerRoom.Application.Repositories;
using InfocomDinnerRoom.Core.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDinnerRoom.Infrastructure.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public MenuRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DefaultConnection is missing in the configuration.");
        }

        public async Task<IReadOnlyList<Menu>> GetAllAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Menu";
                var result = await connection.QueryAsync<Menu>(query);

                return result.ToList();
            }
        }

        public async Task<Menu> AddAsync(Menu entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "INSERT INTO Menu (WeekDay, DishId, WeekId, Cost) VALUES (@WeekDay, @DishId, @WeekId, @Cost) RETURNING Id";
                var id = await connection.ExecuteScalarAsync<int>(query, entity);

                entity.Id = id;

                return entity;
            }
        }

        public async Task<Menu> UpdateAsync(Menu entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "UPDATE Menu SET WeekDay = @WeekDay, DishId = @DishId, WeekId = @WeekId, Cost = @Cost WHERE Id = @Id";
                await connection.ExecuteAsync(query, entity);

                return entity;
            }
        }

        public async Task<Menu> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var menu = await GetByIdAsync(id);
                var query = "DELETE FROM Menu WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });

                return menu;
            }
        }

        private async Task<Menu> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Menu WHERE Id = @Id";
                var result = await connection.QuerySingleOrDefaultAsync<Menu>(query, new { Id = id });

                return result;
            }
        }

        public async Task<List<MenuInfo>> GetMenu(int weekNumber)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
            SELECT m.WeekDay, d.TypeName, d.Name, d.CurrentPrice, d.Description, m.Cost
            FROM Menu m
            JOIN Dishes d ON d.Id = m.DishId
            JOIN Week w ON w.Id = m.WeekId
            WHERE w.WeekNumber = @WeekNumber
            ORDER BY CASE m.WeekDay
                WHEN 'Понеділок' THEN 1
                WHEN 'Вівторок' THEN 2
                WHEN 'Середа' THEN 3
                WHEN 'Четвер' THEN 4
                WHEN 'Пʼятниця' THEN 5
                WHEN 'Субота' THEN 6
                WHEN 'Неділя' THEN 7
                ELSE 8
            END";

                var menuInfoDictionary = new Dictionary<string, MenuInfo>();

                await connection.QueryAsync<MenuInfo, MenuItemInfo, MenuInfo>(
                    query,
                    (menuInfo, menuItem) =>
                    {
                        if (!menuInfoDictionary.TryGetValue(menuInfo.WeekDay, out var existingMenuInfo))
                        {
                            existingMenuInfo = menuInfo;
                            existingMenuInfo.Dishes = new List<MenuItemInfo>();
                            menuInfoDictionary.Add(menuInfo.WeekDay, existingMenuInfo);
                        }

                        existingMenuInfo.Dishes.Add(menuItem);
                        return menuInfo;
                    },
                    new { WeekNumber = weekNumber },
                    splitOn: "TypeName");

                foreach (var menuInfo in menuInfoDictionary.Values)
                {
                    menuInfo.Cost = menuInfo.Dishes.Sum(dish => dish.CurrentPrice);
                }

                return menuInfoDictionary.Values.ToList();
            }
        }
        public async Task UpdateMenuInfo(List<MenuInfo> menuInfoList)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                foreach (var menuInfo in menuInfoList)
                {
                    // Update the Menu cost
                    var updateMenuQuery = @"
                UPDATE Menu
                SET Cost = @Cost
                WHERE WeekDay = @WeekDay";

                    await connection.ExecuteAsync(updateMenuQuery, menuInfo);

                    // Update or insert the Dishes data
                    foreach (var menuItem in menuInfo.Dishes)
                    {
                        var existingDish = await connection.QuerySingleOrDefaultAsync<Dish>(
                            "SELECT * FROM Dishes WHERE Name = @Name",
                            menuItem);

                        if (existingDish != null)
                        {
                            // Dish already exists, perform update
                            var updateDishQuery = @"
                        UPDATE Dishes
                        SET TypeName = @TypeName,
                            CurrentPrice = @CurrentPrice,
                            Description = @Description
                        WHERE Name = @Name";

                            await connection.ExecuteAsync(updateDishQuery, menuItem);
                        }
                        else
                        {
                            // Dish does not exist, perform insert
                            var insertDishQuery = @"
                        INSERT INTO Dishes (Name, TypeName, CurrentPrice, Description)
                        VALUES (@Name, @TypeName, @CurrentPrice, @Description)";

                            await connection.ExecuteAsync(insertDishQuery, menuItem);
                        }
                    }
                }
            }
        }
    }
}
