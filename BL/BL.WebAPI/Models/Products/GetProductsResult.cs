using BL.Core.Domain;

namespace BL.WebAPI.Models.Products
{
    public class GetProductsResult : BaseResult
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
    }
}
