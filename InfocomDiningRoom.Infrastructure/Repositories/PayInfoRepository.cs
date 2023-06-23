using Dapper;
using InfocomDiningRoom.Core.Models.Payment;
using InfocomDinnerRoom.Application.Repositories;
using InfocomDinnerRoom.Core.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Linq;

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

        public async Task<IEnumerable<MenuPaymentInfo>> GetMenuInfo(int weekNumber)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT CONCAT(p.Name, ' ', p.Surname) AS FIO, m.WeekDay, d.TypeName, d.Name, m.Cost, pay.TotalAccured, p.Balance
                            FROM PersonalInfo p
                            JOIN PayInfo pay ON pay.PersonalInfoId = p.Id
                            JOIN Week w ON w.Id = pay.WeekId
                            JOIN Menu m ON m.WeekId = w.Id
                            JOIN Dishes d ON d.Id = m.DishId
                            WHERE w.WeekNumber = @WeekNumber";

                var menuInfoList = new List<MenuPaymentInfo>();

                await connection.QueryAsync<MenuPaymentInfo, string, string, string, decimal, decimal, decimal, MenuPaymentInfo>(
                    query,
                    (info, weekDay, typeName, dishName, currentPrice, totalAccured, balance) =>
                    {
                        if (info.Weeks == null)
                            info.Weeks = new Dictionary<string, Dictionary<string, decimal>>();

                        if (!info.Weeks.TryGetValue(weekDay, out var dayMenu))
                        {
                            dayMenu = new Dictionary<string, decimal>();
                            info.Weeks.Add(weekDay, dayMenu);
                        }

                        dayMenu.Add(dishName, currentPrice);

                        info.TotalCost += currentPrice;
                        info.TotalPaid += totalAccured;
                        info.Balance = balance;

                        menuInfoList.Add(info);

                        return info;
                    },
                    splitOn: "WeekDay,TypeName,Name,Cost,TotalAccured,Balance",
                    param: new { WeekNumber = weekNumber });

                return menuInfoList;
            }
        }
        public async Task<MenuPaymentInfo> UpdateBalance(MenuPaymentInfo menuInfo, decimal newBalance)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
            SELECT Id
            FROM PersonalInfo
            WHERE CONCAT(Name, ' ', Surname) = @FIO";

                var personalInfoId = await connection.ExecuteScalarAsync<int>(query, new { FIO = menuInfo.FIO });

                if (personalInfoId != 0)
                {
                    var updateQuery = @"
                UPDATE PersonalInfo
                SET Balance = @NewBalance
                WHERE Id = @Id";

                    await connection.ExecuteAsync(updateQuery, new { NewBalance = newBalance, Id = personalInfoId });
                }
            }

            menuInfo.Balance = newBalance;
            return menuInfo;
        }
    }
}
