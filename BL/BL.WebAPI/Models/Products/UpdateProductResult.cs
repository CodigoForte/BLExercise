using BL.Core.Domain;

namespace BL.WebAPI.Models.Products
{
    public class UpdateProductResult : BaseResult
    {
        public Product Product { get; set; } = new Product();
    }
}
