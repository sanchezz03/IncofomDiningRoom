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
    public class OrderRulesRepository : IOrderRuleRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public OrderRulesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DbConnection is missing in the configuration.");
        }

        public async Task<IReadOnlyList<OrderRules>> GetAllAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM OrderRules";
                var result = await connection.QueryAsync<OrderRules>(query);

                return result.ToList();
            }
        }

        public async Task<OrderRules> AddAsync(OrderRules entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "INSERT INTO OrderRules (AllowableCredit, OrderDeadline) VALUES (@AllowableCredit, @OrderDeadline) RETURNING Id";
                var id = await connection.ExecuteScalarAsync<int>(query, entity);

                entity.Id = id;

                return entity;
            }
        }

        public async Task<OrderRules> UpdateAsync(OrderRules entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "UPDATE OrderRules SET AllowableCredit = @AllowableCredit, OrderDeadline = @OrderDeadline WHERE Id = @Id";
                await connection.ExecuteAsync(query, entity);

                return entity;
            }
        }

        public async Task<OrderRules> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var orderRules = await GetByIdAsync(id);
                var query = "DELETE FROM OrderRules WHERE Id = @Id";

                await connection.ExecuteAsync(query, new { Id = id });

                return orderRules;
            }
        }

        private async Task<OrderRules> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM OrderRules WHERE Id = @Id";
                var result = await connection.QuerySingleOrDefaultAsync<OrderRules>(query, new { Id = id });

                return result;
            }
        }
    }
}
