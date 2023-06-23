using InfocomDiningRoom.Application.Repositories.Auth;
using InfocomDiningRoom.Application.Repositories.Balance;
using InfocomDiningRoom.Application.Repositories.Management;
using InfocomDiningRoom.Application.Repositories.Request.AdminRequest;
using InfocomDinnerRoom.Application.Repositories;
using InfocomDinnerRoom.Core.Models;
using InfocomDinnerRoom.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDinnerRoom.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IDishRepository dishRepository,
            IOrderRuleRepository orderRuleRepository,
            IRoleRepository roleRepository,
            IWeekRepository weekRepository,
            IPersonalInfoRepository personalInfoRepository,
            IPayInfoRepository payInfoRepository,
            IUserRepository userRepository,
            IMenuRepository menuRepository,
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IAuthRepository authRepository,
            IManagementRepository managementRepository,
            IBalanceRepository balanceRepository,
            IAdminRequestRepository adminRepository
            )
        {
            Dishes = dishRepository;
            OrderRules = orderRuleRepository;
            Roles = roleRepository;
            Weeks = weekRepository;
            PersonalInfos = personalInfoRepository;
            PayInfos = payInfoRepository;
            Users = userRepository;
            Menus = menuRepository;
            Orders = orderRepository;
            OrderDetails = orderDetailRepository;
            Auths = authRepository;
            Managements = managementRepository;
            Balances = balanceRepository;
            Admins = adminRepository;
        }

        public IDishRepository Dishes { get; }
        public IOrderRuleRepository OrderRules { get; }
        public IRoleRepository Roles { get; }
        public IWeekRepository Weeks { get; }
        public IPersonalInfoRepository PersonalInfos { get; }
        public IPayInfoRepository PayInfos { get; }
        public IUserRepository Users { get; }
        public IMenuRepository Menus { get; }
        public IOrderRepository Orders { get; }
        public IOrderDetailRepository OrderDetails { get; }
        public IAuthRepository Auths { get; }
        public IManagementRepository Managements { get; }
        public IBalanceRepository Balances { get; }
        public IAdminRequestRepository Admins { get; }
    }
}
