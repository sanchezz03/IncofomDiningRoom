using InfocomDiningRoom.Core.Models.Payment;
using InfocomDinnerRoom.Core.Models;
using InfocomDiningRoom.Core.Models.Menu;

namespace InfocomDinnerRoom.Application.Repositories
{
    public interface IPayInfoRepository : IGenericRepository<PayInfo>
    {
        Task<IEnumerable<MenuPaymentInfo>> GetMenuInfo(int weekNumber);
        Task<MenuPaymentInfo> UpdateBalance(MenuPaymentInfo menuInfo, decimal newBalance);
    }
}
