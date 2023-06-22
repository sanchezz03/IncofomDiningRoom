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
using static Dapper.SqlMapper;

namespace InfocomDinnerRoom.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DefaultConnection is missing in the configuration.");
        }

        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Users";
                var result = await connection.QueryAsync<User>(query);

                return result.ToList();
            }
        }

        public async Task<User> AddAsync(User entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "INSERT INTO Users (UserName, Password, PersonalInfoId, RoleId) VALUES (@UserName, @Password, @PersonalInfoId, @RoleId) RETURNING Id";
                var id = await connection.ExecuteScalarAsync<int>(query, entity);

                entity.Id = id;

                return entity;
            }
        }

        public async Task<User> UpdateAsync(User entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "UPDATE Users SET UserName = @UserName, Password = @Password, PersonalInfoId = @PersonalInfoId, RoleId = @RoleId WHERE Id = @Id";
                await connection.ExecuteAsync(query, entity);

                return entity;
            }
        }

        public async Task<User> DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var user = await GetByIdAsync(id);
                var query = "DELETE FROM Users WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });

                return user;
            }
        }

        private async Task<User> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Users WHERE Id = @Id";
                var result = await connection.QuerySingleOrDefaultAsync<User>(query, new { Id = id });

                return result;
            }
        }

        public async Task<bool> AddInfoAboutOrdersRule(int allowableCredit, TimeSpan orderDeadline)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"INSERT INTO OrderRules (AllowableCredit, OrderDeadline)
                            VALUES (@AllowableCredit, @OrderDeadline)
                            RETURNING Id";

                    var parameters = new
                    {
                        AllowableCredit = allowableCredit,
                        OrderDeadline = orderDeadline
                    };

                    var id = await connection.ExecuteScalarAsync<int>(query, parameters);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<PersonalInfo>> ShowNotActiveUsers()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = @"SELECT * FROM PersonalInfo WHERE IsActive = false";
                var result = await connection.QueryAsync<PersonalInfo>(query);

                return result;
            }
        }

        public async Task<IEnumerable<PersonalInfo>> SearchUsersByWord(string searchValue)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = @"
            SELECT Id, Name, Surname, Email, IsActive, Balance, CheckCreditLimit
            FROM PersonalInfo
            WHERE 
                CAST(Id AS TEXT) ILIKE @SearchValue OR
                Name ILIKE @SearchValue OR
                Surname ILIKE @SearchValue OR
                Email ILIKE @SearchValue OR
                CAST(IsActive AS TEXT) ILIKE @SearchValue OR
                CAST(Balance AS TEXT) ILIKE @SearchValue OR
                CAST(CheckCreditLimit AS TEXT) ILIKE @SearchValue";

                var parameters = new
                {
                    SearchValue = $"%{searchValue}%" // Include wildcard % to search for partial matches
                };

                var result = await connection.QueryAsync<PersonalInfo>(query, parameters);

                return result;
            }
        }
    }
}
