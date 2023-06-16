using Dapper;
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
    }
}
