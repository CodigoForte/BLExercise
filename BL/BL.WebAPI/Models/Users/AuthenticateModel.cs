namespace BL.WebAPI.Models.Users
{
    public class AuthenticateModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
