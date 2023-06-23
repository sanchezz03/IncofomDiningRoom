using Microsoft.AspNetCore.Mvc;

namespace InfocomDiningRoom.Application.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<string> Login(string email, string password);
    }
}
