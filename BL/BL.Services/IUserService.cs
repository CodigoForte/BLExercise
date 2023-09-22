using BL.Core.Domain;
using BL.Services.Models;

namespace BL.Services
{
    public interface IUserService
    {
        Task<User> AddAsync(User user);

        Task<UserLoginResponse> ValidateCredentialsAsync(string email, string password);
    }
}
