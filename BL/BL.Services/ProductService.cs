using BL.Core.Domain;
using BL.Data;

namespace BL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository _productRepository)
        {
            productRepository = _productRepository;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var products = await productRepository.GetAllAsync();
            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            return product;
        }

        public async Task<Product> AddAsync(Product product)
        {
            ValidateProduct(product);
            return await productRepository.InsertAsync(product);
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            ValidateProduct(product);

            var currentProduct = await GetByIdAsync(product.Id);

            if (currentProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {product.Id} not found.");
            }

            return await productRepository.UpdateAsync(product);
        }

        public async Task RemoveAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            await productRepository.DeleteByIdAsync(id);
        }

        private void ValidateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            ValidateProductName(product.Name);
            ValidateProductDescription(product.Description);
            ValidateProductPrice(product.Price);
        }

        private void ValidateProductName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.");

            if (name.Length < 2 || name.Length > 50)
                throw new ArgumentException("Product name should have between 2 and 50 characters.");
        }

        private void ValidateProductDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Product description cannot be empty.");

            if (description.Length < 20)
                throw new ArgumentException("Product description should have more than 20 characters.");
        }

        private void ValidateProductPrice(decimal price)
        {
            if (price <= 0)
                throw new ArgumentException("The product should have a price above zero.");

            if (price > 10000)
                throw new ArgumentException("The product's price shouldn't exceed 10,000.");
        }
    }
}
