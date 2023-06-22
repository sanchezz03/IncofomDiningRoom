using Dapper;
using InfocomDiningRoom.Application.Repositories.Auth;
using InfocomDinnerRoom.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public AuthRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DefaultConnection is missing in the configuration.");
        }
        public async Task<bool> Login(string email, string password)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var getEmailQuery = "SELECT * FROM PERSONALINFO WHERE email = @Email";

                var getEmailResult = await connection.QueryAsync<PersonalInfo>(getEmailQuery, new { Email = email });

                if (getEmailResult.Count() == 0)
                    return false;

                var getPasswordQuery = "SELECT * FROM Users WHERE password = @Password";

                var getPasswordResult = await connection.QueryAsync<User>(getPasswordQuery, new { Password = password });

                if (getPasswordResult.Count() == 0)
                    return false;

                return true;
            }
        }
    }
}