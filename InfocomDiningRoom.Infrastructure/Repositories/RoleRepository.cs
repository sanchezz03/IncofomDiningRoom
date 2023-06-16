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
    public class RoleRepository : IRoleRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public RoleRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DefaultConnection is missing in the configuration."); 
        }
        public async Task<IReadOnlyList<Role>> GetAllAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Role";
                var result = await connection.QueryAsync<Role>(query);

                return result.ToList();
            }
        }

        public async Task<Role> AddAsync(Role entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "INSERT INTO Role (Name, IsAdmin, StartView) VALUES (@Name, @IsAdmin, @StartView) RETURNING Id";
                var id = await connection.ExecuteScalarAsync<int>(query, entity);

                entity.Id = id;

                return entity;
            }
        }

        public async Task<Role> UpdateAsync(Role entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "UPDATE Role SET Name = @Name, IsAdmin = @IsAdmin, StartView = @StartView WHERE Id = @Id";
                await connection.ExecuteAsync(query, entity);

                return entity;
            }
        }

        public async Task<Role> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var role = await GetByIdAsync(id);
                var query = "DELETE FROM Role WHERE Id = @Id";

                await connection.ExecuteAsync(query, new { Id = id });

                return role;
            }
        }

        private async Task<Role> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Role WHERE Id = @Id";
                var result = await connection.QuerySingleOrDefaultAsync<Role>(query, new { Id = id });

                return result;
            }
        }
    }
}
