namespace BL.WebAPI.Models
{
    public class BaseResult
    {
        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;
    }
}
