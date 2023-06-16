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
    public class PersonalInfoRepository : IPersonalInfoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public PersonalInfoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DefaultConnection is missing in the configuration."); 
        }

        public async Task<IReadOnlyList<PersonalInfo>> GetAllAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM PersonalInfo";
                var result = await connection.QueryAsync<PersonalInfo>(query);

                return result.ToList();
            }
        }

        public async Task<PersonalInfo> AddAsync(PersonalInfo entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "INSERT INTO PersonalInfo (Name, Surname, Email, IsActive, Balance, CheckCreditLimit) VALUES (@Name, @Surname, @Email, @IsActive, @Balance, @CheckCreditLimit) RETURNING Id";
                var id = await connection.ExecuteScalarAsync<int>(query, entity);

                entity.Id = id;

                return entity;
            }
        }

        public async Task<PersonalInfo> UpdateAsync(PersonalInfo entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "UPDATE PersonalInfo SET Name = @Name, Surname = @Surname, Email = @Email, IsActive = @IsActive, Balance = @Balance, CheckCreditLimit = @CheckCreditLimit WHERE Id = @Id";
                await connection.ExecuteAsync(query, entity);

                return entity;
            }
        }

        public async Task<PersonalInfo> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var personalInfo = await GetByIdAsync(id);
                var query = "DELETE FROM PersonalInfo WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });

                return personalInfo;
            }
        }

        private async Task<PersonalInfo> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM PersonalInfo WHERE Id = @Id";
                var result = await connection.QuerySingleOrDefaultAsync<PersonalInfo>(query, new { Id = id });

                return result;
            }
        }
    }
}
