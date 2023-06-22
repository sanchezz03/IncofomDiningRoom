using Microsoft.AspNetCore.Mvc;

namespace InfocomDiningRoom.Application.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<bool> Login(string email, string password);
    }
}
