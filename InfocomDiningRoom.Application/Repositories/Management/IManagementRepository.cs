using InfocomDiningRoom.Core.Models.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Application.Repositories.Management
{
    public interface IManagementRepository
    {
        Task<IEnumerable<UserInfo>> Users();
        Task<bool> EditUser(int userId, string newUserName, string newEmail, int newRoleId);
        Task<IEnumerable<RoleInfo>> Roles();
        Task<bool> EditRole(RoleInfo roleInfo);

    }
}
