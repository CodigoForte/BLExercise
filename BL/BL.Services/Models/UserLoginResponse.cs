using BL.Core.Domain;

namespace BL.Services.Models
{
    public class UserLoginResponse
    {
        public UserLoginStates State { get; set; }

        public UserWithoutPassword User { get; set; }
    }
}
