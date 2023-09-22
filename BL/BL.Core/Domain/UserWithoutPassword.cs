namespace BL.Core.Domain
{
    public class UserWithoutPassword
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
