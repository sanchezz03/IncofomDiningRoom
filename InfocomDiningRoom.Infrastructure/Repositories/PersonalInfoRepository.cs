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
    public class WeekRepository : IWeekRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public WeekRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DefaultConnection is missing in the configuration."); 
        }

        public async Task<IReadOnlyList<Week>> GetAllAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Week";
                var result = await connection.QueryAsync<Week>(query);

                return result.ToList();
            }
        }

        public async Task<Week> AddAsync(Week entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "INSERT INTO Week (StartDate, EndDate, Year, WeekNumber) VALUES (@StartDate, @EndDate, @Year, @WeekNumber) RETURNING Id";
                var id = await connection.ExecuteScalarAsync<int>(query, entity);

                entity.Id = id;

                return entity;
            }
        }

        public async Task<Week> UpdateAsync(Week entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "UPDATE Week SET StartDate = @StartDate, EndDate = @EndDate, Year = @Year, WeekNumber = @WeekNumber WHERE Id = @Id";
                await connection.ExecuteAsync(query, entity);

                return entity;
            }
        }

        public async Task<Week> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var week = await GetByIdAsync(id);
                var query = "DELETE FROM Week WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });

                return week;
            }
        }

        private async Task<Week> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Week WHERE Id = @Id";
                var result = await connection.QuerySingleOrDefaultAsync<Week>(query, new { Id = id });

                return result;
            }
        }
    }
}
