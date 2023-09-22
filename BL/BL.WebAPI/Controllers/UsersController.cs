using BL.Core.Domain;
using BL.Services;
using BL.Services.Models;
using BL.WebAPI.Models.Users;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BL.WebAPI.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UsersController(ILogger<ProductsController> logger, IUserService userService, IConfiguration configuration)
        {
            _logger = logger;
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Add(NewUser newUser)
        {
            AddUserResult result = new();

            try
            {
                var user = new User()
                {
                    Name = newUser.Name,
                    Email = newUser.Email,
                    Password = newUser.Password,
                };

                var addedUser = await _userService.AddAsync(user);

                result.IsSuccess = true;
                result.User = new UserWithoutPassword() 
                { 
                    Id = addedUser.Id, 
                    Email = addedUser.Email, 
                    Name = addedUser.Name,
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;

                return BadRequest(result);
            }
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            ValidateCredentialsResult result = new();

            try
            {
                UserLoginResponse response = await _userService.ValidateCredentialsAsync(model.Email, model.Password);

                switch (response.State)
                {
                    case UserLoginStates.Ok:
                        var token = GenerateJwtToken(response.User.Id.ToString(), model.Email);
                        result.Token = token;
                        return Ok(result);

                    case UserLoginStates.WrongPassword:
                        result.IsSuccess = false;
                        result.ErrorMessage = "Credentials are not valid.";
                        return BadRequest(result);

                    case UserLoginStates.EmailNotRegistered:
                        result.IsSuccess = false;
                        result.ErrorMessage = "Email is not registered.";
                        return NotFound(result);

                    default:
                        result.IsSuccess = false;
                        result.ErrorMessage = "Unexpected error. Unhandled response State.";
                        return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;

                return BadRequest(result);
            }
        }

        private string GenerateJwtToken(string userId, string email)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}