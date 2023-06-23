using Dapper;
using InfocomDiningRoom.Application.Repositories.Auth;
using InfocomDinnerRoom.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
        public async Task<string> Login(string userName, string password)
        {
            string token = "";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var getUserQuery = @"
                            SELECT u.*, r.Name AS Role
                            FROM Users u
                            JOIN Role r ON r.Id = u.RoleId
                            WHERE u.username = @UserName AND u.password = @Password";

                var getUserResult = await connection.QueryAsync<InfocomDiningRoom.Core.Models.Auth.User>(getUserQuery, new { UserName = userName, Password = password  });

                var user = getUserResult.SingleOrDefault();

                if (user != null)
                {
                    token = GenerateToken(user.userName, user.role);
                }

                return token;
            }
        }

        private string GenerateToken(string userName, string role)
        {
            List<Claim> claims = new List<Claim>()
    {
        new Claim(ClaimTypes.NameIdentifier, userName),
        new Claim(ClaimTypes.Role, role)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetValue<string>("SecretToken:Token"))); 

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}