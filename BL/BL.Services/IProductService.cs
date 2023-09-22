using BL.Core.Domain;

namespace BL.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();

        Task<Product> GetByIdAsync(int id);

        Task<Product> AddAsync(Product product);

        Task<Product> UpdateAsync(Product product);

        Task RemoveAsync(int id);
    }
}
