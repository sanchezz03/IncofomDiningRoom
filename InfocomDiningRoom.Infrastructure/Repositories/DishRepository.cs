using Dapper;
using InfocomDiningRoom.Core.Models.Dish;
using InfocomDinnerRoom.Application.Repositories;
using InfocomDinnerRoom.Core.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDinnerRoom.Infrastructure.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DishRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DefaultConnection is missing in the configuration."); 
        }

        public async Task<IReadOnlyList<Dish>> GetAllAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Dishes";
                var result = await connection.QueryAsync<Dish>(query);

                return result.ToList();
            }
        }

        public async Task<Dish> AddAsync(Dish entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "INSERT INTO Dishes (Name, TypeName, CurrentPrice, Description) VALUES (@Name, @TypeName, @CurrentPrice, @Description) RETURNING Id";
                var id = await connection.ExecuteScalarAsync<int>(query, entity);

                entity.Id = id;

                return entity;
            }
        }

        public async Task<Dish> UpdateAsync(Dish entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "UPDATE Dishes SET Name = @Name, TypeName = @TypeName, CurrentPrice = @CurrentPrice, Description = @Description WHERE Id = @Id";
                await connection.ExecuteAsync(query, entity);

                return entity;
            }
        }

        public async Task<Dish> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var dish = await GetByIdAsync(id);
                var query = "DELETE FROM Dishes WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });

                return dish;
            }
        }

        private async Task<Dish> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Dishes WHERE Id = @Id";
                var result = await connection.QuerySingleOrDefaultAsync<Dish>(query, new { Id = id });

                return result;
            }
        }

        public async Task<IReadOnlyList<DishInfo>> GetFirstDishes()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
            SELECT Id, CurrentPrice, Name, Description
            FROM Dishes
            WHERE TypeName = 'Суп'";

                var dishes = await connection.QueryAsync<DishInfo>(query);
                return dishes.ToList();
            }
        }

        public async Task<IReadOnlyList<DishInfo>> GetSecondDishes()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
            SELECT Id, CurrentPrice, Name, Description
            FROM Dishes
            WHERE TypeName = 'Головна страва'";

                var dishes = await connection.QueryAsync<DishInfo>(query);

                return dishes.ToList();
            }
        }

        public async Task<IReadOnlyList<DishInfo>> GetSalad()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
            SELECT Id, CurrentPrice, Name, Description
            FROM Dishes
            WHERE TypeName = 'Салат'";

                var dishes = await connection.QueryAsync<DishInfo>(query);

                return dishes.ToList();
            }
        }

        public async Task<IReadOnlyList<DishInfo>> GetDrinks()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
            SELECT Id, CurrentPrice, Name, Description
            FROM Dishes
            WHERE TypeName = 'Напій'";

                var dishes = await connection.QueryAsync<DishInfo>(query);

                return dishes.ToList();
            }
        }
    }
}
