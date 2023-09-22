namespace BL.Core.Domain
{
    public class User : UserWithoutPassword
    {
        public string Password { get; set; } = string.Empty;
    }
}
