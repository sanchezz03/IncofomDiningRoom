using InfocomDiningRoom.Core.Models.Balance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Application.Repositories.Balance
{
    public interface IBalanceRepository
    {
        Task<List<UserData>> GetBalance(int startWeekNumber, int endWeekNumber);
    }
}
