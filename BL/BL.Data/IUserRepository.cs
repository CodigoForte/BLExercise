using BL.Core.Domain;

namespace BL.Data
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);

        Task<User> InsertAsync(User user);
    }
}
