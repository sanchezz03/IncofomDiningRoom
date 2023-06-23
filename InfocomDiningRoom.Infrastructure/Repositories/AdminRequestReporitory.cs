using Dapper;
using InfocomDiningRoom.Application.Repositories.Request.AdminRequest;
using InfocomDiningRoom.Core.Models.Menu;
using InfocomDiningRoom.Core.Models.Payment;
using InfocomDiningRoom.Core.Models.Request.AdminRequest;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Infrastructure.Repositories
{
    public class AdminRequestReporitory : IAdminRequestRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public AdminRequestReporitory(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DefaultConnection is missing in the configuration.");
        }

        public async Task<MenuPaymentInfoRequest> GetMenuPaymentInfo(int weekNumber)
        {
            var query = @"
        SELECT
            p.Name AS FIO,
            m.WeekDay,
            d.Name AS DishName,
            SUM(m.Cost * od.Count) AS TotalCost
        FROM
            PersonalInfo p
            LEFT JOIN PayInfo pi ON p.id = pi.PersonalInfoId
            LEFT JOIN Week w ON w.id = pi.WeekId
            LEFT JOIN OrderDetails od ON od.OrderId = pi.id
            LEFT JOIN Dishes d ON d.id = od.DishId
            LEFT JOIN Menu m ON m.DishId = d.id AND m.WeekId = w.id
        WHERE
            w.WeekNumber = @WeekNumber
        GROUP BY
            p.Name,
            m.WeekDay,
            d.Name
        ORDER BY
            p.Name,
            m.WeekDay,
            d.Name";

            var menuInfoDictionary = new Dictionary<string, MenuInfoRequest>();
            var menuPaymentInfoRequest = new MenuPaymentInfoRequest
            {
                MenuInfoRequestList = new List<MenuInfoRequest>()
            };

            await using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var menuInfo = (await connection.QueryAsync<MenuInfoRequest, string, string, decimal, MenuInfoRequest>(
                    query,
                    (menu, weekDay, dishName, totalCost) =>
                    {
                        if (!menuInfoDictionary.TryGetValue(menu.FIO, out var menuInfoRequest))
                        {
                            menuInfoRequest = new MenuInfoRequest
                            {
                                FIO = menu.FIO,
                                Weeks = new Dictionary<string, Dictionary<string, int>>()
                            };
                            menuInfoDictionary[menu.FIO] = menuInfoRequest;
                        }

                        if (!menuInfoRequest.Weeks.TryGetValue(weekDay, out var dayMenu))
                        {
                            dayMenu = new Dictionary<string, int>();
                            menuInfoRequest.Weeks[weekDay] = dayMenu;
                        }

                        dayMenu[dishName] = (int)totalCost;

                        return menu;
                    },
                    splitOn: "FIO,WeekDay,DishName,TotalCost",
                    param: new { WeekNumber = weekNumber })
                ).Distinct().ToList();

                foreach (var menuInfoRequest in menuInfoDictionary.Values)
                {
                    var sumOfAllCostOrders = menuInfoRequest.Weeks.Values.Sum(x => x.Values.Sum());
                    menuInfoRequest.Weeks.Add("sumOfAllCostOrders", new Dictionary<string, int> { { "sumOfAllCostOrders", sumOfAllCostOrders } });
                }

                menuPaymentInfoRequest.MenuInfoRequestList.AddRange(menuInfoDictionary.Values);
            }

            return menuPaymentInfoRequest;
        }
    }
}
