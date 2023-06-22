using InfocomDiningRoom.Core.Models.Menu;
using InfocomDinnerRoom.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDinnerRoom.Application.Repositories
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {
        Task<List<MenuInfo>> GetMenu(int weekNumber);
        Task UpdateMenuInfo(List<MenuInfo> menuInfoList);
    }
}
