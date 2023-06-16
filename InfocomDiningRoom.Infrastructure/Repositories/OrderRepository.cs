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
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DefaultConnection is missing in the configuration.");
        }

        public async Task<IReadOnlyList<Order>> GetAllAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Orders";
                var result = await connection.QueryAsync<Order>(query);

                return result.ToList();
            }
        }

        public async Task<Order> AddAsync(Order entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "INSERT INTO Orders (MenuId, UserId, Cost, OrderRulesId) VALUES (@MenuId, @UserId, @Cost, @OrderRulesId) RETURNING Id";
                var id = await connection.ExecuteScalarAsync<int>(query, entity);

                entity.Id = id;

                return entity;
            }
        }

        public async Task<Order> UpdateAsync(Order entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "UPDATE Orders SET MenuId = @MenuId, UserId = @UserId, Cost = @Cost, OrderRulesId = @OrderRulesId WHERE Id = @Id";
                await connection.ExecuteAsync(query, entity);

                return entity;
            }
        }

        public async Task<Order> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var order = await GetByIdAsync(id);
                var query = "DELETE FROM Orders WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });

                return order;
            }
        }

        private async Task<Order> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Orders WHERE Id = @Id";
                var result = await connection.QuerySingleOrDefaultAsync<Order>(query, new { Id = id });

                return result;
            }
        }
    }
}
