using BL.Core.Domain;

namespace BL.WebAPI.Models.Products
{
    public class GetProductResult : BaseResult
    {
        public Product Product { get; set; } = new Product();
    }
}
