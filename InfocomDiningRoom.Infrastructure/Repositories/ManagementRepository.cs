using Dapper;
using InfocomDiningRoom.Application.Repositories.Management;
using InfocomDiningRoom.Core.Models.Management;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Infrastructure.Repositories
{
    public class ManagementRepository : IManagementRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ManagementRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DefaultConnection is missing in the configuration.");
        }
        public async Task<IEnumerable<UserInfo>> Users()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = @"SELECT u.id, u.username, p.Email, r.Name AS Role
                             FROM Users u
                             JOIN Role r ON r.id = u.roleid
			                 JOIN PersonalInfo p on p.id = u.personalinfoid";

                var result = await connection.QueryAsync<UserInfo>(query);

                if (result.Count() == 0)
                    return null;

                return result.ToList();
            }
        }
        public async Task<bool> EditUser(int userId, string newUserName, string newEmail, int newRoleId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var updateUserQuery = @"
                                UPDATE Users
                                SET UserName = @NewUserName, RoleId = @NewRoleId
                                WHERE id = @UserId";

                        var updateUserParams = new
                        {
                            NewUserName = newUserName,
                            NewRoleId = newRoleId,
                            UserId = userId
                        };

                        await connection.ExecuteAsync(updateUserQuery, updateUserParams, transaction);

                        var updatePersonalInfoQuery = @"
                            UPDATE PersonalInfo
                            SET Email = @NewEmail
                            WHERE id = (
                                SELECT PersonalInfoId
                                FROM Users
                                WHERE id = @UserId
                                )";

                        var updatePersonalInfoParams = new
                        {
                            NewEmail = newEmail,
                            UserId = userId
                        };

                        await connection.ExecuteAsync(updatePersonalInfoQuery, updatePersonalInfoParams, transaction);

                        transaction.Commit();

                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public async Task<IEnumerable<RoleInfo>> Roles()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = @"SELECT r.id, u.UserName, r.Name AS RoleName
                      FROM Role r
                      JOIN Users u ON r.id = u.RoleId";

                var result = await connection.QueryAsync<RoleInfo>(query);

                return result;
            }
        }
        public async Task<bool> EditRole(RoleInfo roleInfo)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var updateRoleQuery = @"UPDATE Role
                                                SET Name = @RoleName
                                                WHERE id = @RoleId";

                        var updateRoleParams = new
                        {
                            RoleName = roleInfo.RoleName,
                            RoleId = roleInfo.Id
                        };

                        await connection.ExecuteAsync(updateRoleQuery, updateRoleParams, transaction);

                        var updateUserQuery = @"UPDATE Users
                                                SET UserName = @UserName
                                                WHERE RoleId = @RoleId";

                        var updateUserParams = new
                        {
                            UserName = roleInfo.UserName,
                            RoleId = roleInfo.Id
                        };

                        await connection.ExecuteAsync(updateUserQuery, updateUserParams, transaction);

                        transaction.Commit();

                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
