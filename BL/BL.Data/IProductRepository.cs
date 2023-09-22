using BL.Core.Domain;

namespace BL.Data
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();

        Task<Product> GetByIdAsync(int id);

        Task<Product> InsertAsync(Product product);

        Task<Product> UpdateAsync(Product product);

        Task DeleteByIdAsync(int id);
    }
}
