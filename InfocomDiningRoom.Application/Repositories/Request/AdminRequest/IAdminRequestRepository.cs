using InfocomDiningRoom.Core.Models.Dish;
using InfocomDiningRoom.Core.Models.Payment;
using InfocomDiningRoom.Core.Models.Request.AdminRequest;
using InfocomDinnerRoom.Application.Repositories;
using InfocomDinnerRoom.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Application.Repositories.Request.AdminRequest
{
    public interface IAdminRequestRepository
    {
        Task<MenuPaymentInfoRequest> GetMenuPaymentInfo(int weekNumber);
    }
}
