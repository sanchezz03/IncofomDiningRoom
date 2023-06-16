using Dapper;
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
    public class PayInfoRepository : IPayInfoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public PayInfoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DefaultConnection is missing in the configuration."); 
        }

        public async Task<IReadOnlyList<PayInfo>> GetAllAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM PayInfo";
                var result = await connection.QueryAsync<PayInfo>(query);

                return result.ToList();
            }
        }

        public async Task<PayInfo> AddAsync(PayInfo entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "INSERT INTO PayInfo (TotalAccrued, TotalPaid, WeekId, PersonalInfoId) VALUES (@TotalAccrued, @TotalPaid, @WeekId, @PersonalInfoId) RETURNING Id";
                var id = await connection.ExecuteScalarAsync<int>(query, entity);

                entity.Id = id;

                return entity;
            }
        }

        public async Task<PayInfo> UpdateAsync(PayInfo entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "UPDATE PayInfo SET TotalAccrued = @TotalAccrued, TotalPaid = @TotalPaid, WeekId = @WeekId, PersonalInfoId = @PersonalInfoId WHERE Id = @Id";
                await connection.ExecuteAsync(query, entity);

                return entity;
            }
        }

        public async Task<PayInfo> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var payInfo = await GetByIdAsync(id);
                var query = "DELETE FROM PayInfo WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });

                return payInfo;
            }
        }

        private async Task<PayInfo> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM PayInfo WHERE Id = @Id";
                var result = await connection.QuerySingleOrDefaultAsync<PayInfo>(query, new { Id = id });

                return result;
            }
        }
    }
}
