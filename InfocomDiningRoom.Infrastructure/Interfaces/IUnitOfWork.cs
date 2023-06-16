using InfocomDinnerRoom.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDinnerRoom.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IDishRepository Dishes { get; }
        IOrderRuleRepository OrderRules { get; }
        IRoleRepository Roles { get; }
        IWeekRepository Weeks { get; }
        IPersonalInfoRepository PersonalInfos { get; }
        IPayInfoRepository PayInfos { get; }
        IUserRepository Users { get; }
        IMenuRepository Menus { get; }
        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; }
    }
}
