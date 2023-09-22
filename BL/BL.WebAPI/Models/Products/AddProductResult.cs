using BL.Core.Domain;

namespace BL.WebAPI.Models.Products
{
    public class AddProductResult : BaseResult
    {
        public Product Product { get; set; } = new Product();
    }
}
