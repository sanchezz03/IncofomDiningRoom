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
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public OrderDetailRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DefaultConnection is missing in the configuration.");
        }

        public async Task<IReadOnlyList<OrderDetails>> GetAllAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM OrderDetails";
                var result = await connection.QueryAsync<OrderDetails>(query);

                return result.ToList();
            }
        }

        public async Task<OrderDetails> AddAsync(OrderDetails entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "INSERT INTO OrderDetails (DishId, OrderId, Count) VALUES (@DishId, @OrderId, @Count) RETURNING Id";
                var id = await connection.ExecuteScalarAsync<int>(query, entity);

                entity.Id = id;

                return entity;
            }
        }

        public async Task<OrderDetails> UpdateAsync(OrderDetails entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "UPDATE OrderDetails SET DishId = @DishId, OrderId = @OrderId, Count = @Count WHERE Id = @Id";
                await connection.ExecuteAsync(query, entity);

                return entity;
            }
        }

        public async Task<OrderDetails> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var orderDetails = await GetByIdAsync(id);
                var query = "DELETE FROM OrderDetails WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });

                return orderDetails;
            }
        }

        private async Task<OrderDetails> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM OrderDetails WHERE Id = @Id";
                var result = await connection.QuerySingleOrDefaultAsync<OrderDetails>(query, new { Id = id });

                return result;
            }
        }
    }
}
