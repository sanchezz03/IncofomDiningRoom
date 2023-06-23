using InfocomDiningRoom.Application.Repositories.Balance;
using InfocomDiningRoom.Core.Models.Balance;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Text.Json;


namespace InfocomDiningRoom.Infrastructure.Repositories
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public BalanceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DefaultConnection is missing in the configuration.");
        }
        public async Task<List<UserData>> GetBalance(int startWeekNumber, int endWeekNumber)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                SELECT
                    CONCAT(p.Name, ' ', p.Surname) AS FullName,
                    COALESCE(jsonb_agg(jsonb_build_object('WeekNumber', w.WeekNumber, 'TotalAccured', pi.TotalAccured, 'TotalPaid', pi.TotalPaid)), '[]'::jsonb) AS WeeksData,
                    SUM(pi.TotalAccured) AS SumAccured,
                    SUM(pi.TotalPaid) AS SumTotalPaid
                FROM
                    PersonalInfo p
                    JOIN PayInfo pi ON p.Id = pi.PersonalInfoId
                    JOIN Week w ON pi.WeekId = w.Id
                WHERE
                    w.WeekNumber >= @startWeekNumber AND w.WeekNumber <= @endWeekNumber
                GROUP BY
                    p.Name, p.Surname
                ORDER BY
                    p.Name, p.Surname";

                    command.Parameters.AddWithValue("@startWeekNumber", startWeekNumber);
                    command.Parameters.AddWithValue("@endWeekNumber", endWeekNumber);

                    var userDataList = new List<UserData>();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var fullName = reader.GetString(0);
                            var weeksDataJson = reader.GetString(1);

                            var jsonOptions = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            };

                            var weeksData = string.IsNullOrEmpty(weeksDataJson)
                                ? new List<WeekData>()
                                : JsonSerializer.Deserialize<List<WeekData>>(weeksDataJson, jsonOptions);

                            var userData = new UserData
                            {
                                FullName = fullName,
                                WeeksData = weeksData,
                                SumAccured = reader.GetFieldValue<decimal>(2),
                                SumTotalPaid = reader.GetFieldValue<decimal>(3),
                                Balance = reader.GetFieldValue<decimal>(3) - reader.GetFieldValue<decimal>(2)
                            };

                            userDataList.Add(userData);
                        }
                    }

                    return userDataList;
                }
            }
        }
    }
}
