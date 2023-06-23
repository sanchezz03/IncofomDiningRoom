using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Application.Repositories.Request.UserRequest
{
    public interface IUserRequestRepository
    {
        Task<List<Dictionary<string, object>>> GetMenuAndTotalCost(int weekNumber);
        Task UpdateOrderDetailsCount(string dishName, int newCount);
        Task<decimal> UpdateUserBalanceBasedOnOrderCost(int weekNumber);
    }
}
