namespace BL.WebAPI.Models.Users
{
    public class ValidateCredentialsResult : BaseResult
    {
        public string Token { get; set; } = string.Empty;
    }
}
