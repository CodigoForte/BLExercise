using BL.Core.Domain;

namespace BL.WebAPI.Models.Users
{
    public class AddUserResult : BaseResult
    {
        public UserWithoutPassword User { get; set; } = new UserWithoutPassword();
    }
}
