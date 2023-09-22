using BL.Core.Domain;
using BL.Core.Security;
using BL.Data;
using BL.Services.Models;
using System.ComponentModel.DataAnnotations;

namespace BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        public async Task<User> AddAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.Name = user.Name.Trim();
            user.Email = user.Email.Trim();
            user.Password = user.Password.Trim();

            if (!new EmailAddressAttribute().IsValid(user.Email))
            {
                throw new ArgumentException("Invalid Email format.");
            }

            var existingUser = await userRepository.GetByEmailAsync(user.Email);

            if (existingUser != null)
            {
                throw new ArgumentException("Email is already registered.");
            }

            // encrypt password
            user.Password = CryptoPassword.CreateHash(user.Password);

            var addedUser = await userRepository.InsertAsync(user);

            // clear password before return
            addedUser.Password = string.Empty;

            return addedUser;
        }

        public async Task<UserLoginResponse> ValidateCredentialsAsync(string email, string password)
        {
            var user = await userRepository.GetByEmailAsync(email);

            if (user == null || user.Id == 0)
            {
                return new UserLoginResponse() { State = UserLoginStates.EmailNotRegistered };
            }

            bool passwordOk = CryptoPassword.ValidatePassword(password, user.Password);

            if (!passwordOk)
            {
                return new UserLoginResponse() { State = UserLoginStates.WrongPassword };
            }

            // if everything is OK, return the user.
            return new UserLoginResponse()
            {
                State = UserLoginStates.Ok,
                User = new UserWithoutPassword()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                }
            };
        }
    }
}
