using InfocomDinnerRoom.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDinnerRoom.Application.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> AddInfoAboutOrdersRule(int allowableCredit, TimeSpan orderDeadline);
        Task<IEnumerable<PersonalInfo>> ShowNotActiveUsers();
        Task<IEnumerable<PersonalInfo>> SearchUsersByWord(string searchValue);
    }
}
