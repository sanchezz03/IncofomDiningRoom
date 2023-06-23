using Dapper;
using InfocomDiningRoom.Application.Repositories.Request.UserRequest;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;

namespace InfocomDiningRoom.Infrastructure.Repositories
{
    public class UserRequestRepository : IUserRequestRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public UserRequestRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DefaultConnection is missing in the configuration.");
        }
        public async Task<List<Dictionary<string, object>>> GetMenuAndTotalCost(int weekNumber)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
            SELECT m.weekday,
                   json_agg(json_build_object('Dish', COALESCE(d.typename, ''), 'Name', COALESCE(d.name, ''), 'Cost', COALESCE(m.cost, 0), 'Count', COALESCE(o.count, 0))) AS dishes,
                   json_build_object('totalcost', COALESCE(SUM(m.cost * o.count), 0)) AS SumCost
            FROM dishes d
            JOIN menu m ON d.id = m.dishid
            JOIN orderdetails o ON o.dishid = d.id
            GROUP BY m.weekday;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    var reader = command.ExecuteReader();

                    var result = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        var weekday = reader["weekday"].ToString();
                        var dishesJson = reader["dishes"].ToString();
                        var sumCostJson = reader["SumCost"].ToString();

                        var dishes = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(dishesJson);
                        var sumCost = JsonConvert.DeserializeObject<Dictionary<string, object>>(sumCostJson);

                        var menu = new Dictionary<string, object>
                {
                    { "weekday", weekday },
                    { "dishes", dishes },
                    { "SumCost", sumCost }
                };

                        result.Add(menu);
                    }

                    return result;
                }
            }
        }
        public async Task UpdateOrderDetailsCount(string dishName, int newCount)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
            UPDATE OrderDetails
            SET Count = @NewCount
            WHERE DishId = (SELECT id FROM Dishes WHERE Name = @DishName)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("NewCount", newCount);
                    command.Parameters.AddWithValue("DishName", dishName);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<decimal> UpdateUserBalanceBasedOnOrderCost(int weekNumber)
        {
            decimal orderCost = 0;
            decimal userBalance = 0;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Retrieve the order cost
                string orderCostQuery = @"
            SELECT SUM(m.Cost * od.Count) AS total_cost
            FROM OrderDetails od
            JOIN Dishes d ON od.DishId = d.Id
            JOIN Menu m ON d.Id = m.DishId
            WHERE m.WeekDay = (SELECT WeekDay FROM Week WHERE Id = @WeekId)";

                using (var command = new NpgsqlCommand(orderCostQuery, connection))
                {
                    command.Parameters.AddWithValue("WeekId", weekNumber);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            orderCost = Convert.ToDecimal(reader["total_cost"]);
                        }
                    }
                }

                // Retrieve the user balance
                string userBalanceQuery = @"
            SELECT Balance
            FROM PersonalInfo
            WHERE Id IN (
                SELECT PersonalInfoId
                FROM Users
            )";

                using (var command = new NpgsqlCommand(userBalanceQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userBalance = Convert.ToDecimal(reader["Balance"]);
                        }
                    }
                }

                // Update the user balance
                userBalance -= orderCost;

                // Update the user balance in the database
                string updateBalanceQuery = @"
            UPDATE PersonalInfo
            SET Balance = @Balance
            WHERE Id IN (
                SELECT PersonalInfoId
                FROM Users
            )";

                using (var command = new NpgsqlCommand(updateBalanceQuery, connection))
                {
                    command.Parameters.AddWithValue("Balance", userBalance);

                    command.ExecuteNonQuery();
                }
            }

            return userBalance;
        }

    }
}
