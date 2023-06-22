using InfocomDiningRoom.Application.Repositories.Auth;
using InfocomDiningRoom.Application.Repositories.Management;
using InfocomDiningRoom.Infrastructure.Repositories;
using InfocomDinnerRoom.Application.Repositories;
using InfocomDinnerRoom.Infrastructure.Interfaces;
using InfocomDinnerRoom.Infrastructure.Repositories;

namespace InfocomDinnerRoom.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDishRepository, DishRepository>();
            services.AddTransient<IOrderRuleRepository, OrderRulesRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IWeekRepository, WeekRepository>();
            services.AddTransient<IPersonalInfoRepository, PersonalInfoRepository>();
            services.AddTransient<IPayInfoRepository, PayInfoRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IMenuRepository, MenuRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderDetailRepository, OrderDetailRepository>();
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IManagementRepository, ManagementRepository>();
        }
    }
}
