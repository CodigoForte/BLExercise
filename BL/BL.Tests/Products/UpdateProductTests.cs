using BL.Core.Domain;
using BL.Data;
using BL.Services;
using Moq;

namespace BL.Tests.Products
{
    public class UpdateProductTests
    {
        private readonly Mock<IProductRepository> mockProductRepo;
        private readonly IProductService productService;

        public UpdateProductTests()
        {
            mockProductRepo = new Mock<IProductRepository>();
            productService = new ProductService(mockProductRepo.Object);
        }

        [Fact]
        public async Task Price_Should_Be_Greater_Than_Zero_On_UpdateProduct()
        {
            // Arrange
            var productWithNegativePrice = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description for the product",
                Price = -1,
            };

            var productWithPrice0 = new Product
            {
                Id = 2,
                Name = "Test Product",
                Description = "Test Description for the product",
                Price = 0,
            };

            // Act
            var exceptionWithNegativePrice = await Assert.ThrowsAsync<ArgumentException>(() => productService.UpdateAsync(productWithNegativePrice));
            var exceptionWithPrice0 = await Assert.ThrowsAsync<ArgumentException>(() => productService.UpdateAsync(productWithPrice0));

            // Assert
            Assert.Equal("The product should have a price above zero.", exceptionWithNegativePrice.Message);
            Assert.Equal("The product should have a price above zero.", exceptionWithPrice0.Message);
        }

        [Fact]
        public async Task Price_Should_Not_Be_Greater_Than_10000_On_UpdateProduct()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description for the product",
                Price = 10001,
            };

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => productService.UpdateAsync(product));

            // Assert
            Assert.Equal("The product's price shouldn't exceed 10,000.", exception.Message);
        }

        [Fact]
        public async Task Product_Name_Should_Not_Be_Empty_On_UpdateProduct()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "",
                Description = "Test Description for the product",
                Price = 10,
            };

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => productService.UpdateAsync(product));

            // Assert
            Assert.Equal("Product name cannot be empty.", exception.Message);
        }

        [Fact]
        public async Task Product_Name_Should_Be_Between_2_And_50_Characters_On_UpdateProduct()
        {
            // Arrange
            var shortNameProduct = new Product
            {
                Id = 1,
                Name = "A",
                Description = "Test Description for the product",
                Price = 10,
            };

            var longNameProduct = new Product
            {
                Id = 2,
                Name = new string('A', 51),
                Description = "Test Description for the product",
                Price = 10,
            };

            var correctNameProduct = new Product
            {
                Id = 3,
                Name = "Test Product",
                Description = "Test Description for the product",
                Price = 10,
            };

            mockProductRepo.Setup(repo => repo.GetByIdAsync(correctNameProduct.Id)).ReturnsAsync(correctNameProduct);
            mockProductRepo.Setup(repo => repo.UpdateAsync(correctNameProduct)).ReturnsAsync(correctNameProduct);

            // Act
            var exceptionShortName = await Assert.ThrowsAsync<ArgumentException>(() => productService.UpdateAsync(shortNameProduct));
            var exceptionLongName = await Assert.ThrowsAsync<ArgumentException>(() => productService.UpdateAsync(longNameProduct));
            var updatedProduct = await productService.UpdateAsync(correctNameProduct);

            // Assert
            Assert.Equal("Product name should have between 2 and 50 characters.", exceptionShortName.Message);
            Assert.Equal("Product name should have between 2 and 50 characters.", exceptionLongName.Message);
            Assert.Equal(correctNameProduct.Name, updatedProduct.Name);
        }

        [Fact]
        public async Task Product_Description_Should_Not_Be_Empty_And_Have_Min_20_Characters_On_UpdateProduct()
        {
            // Arrange
            var emptyDescriptionProduct = new Product
            {
                Name = "Test Product",
                Description = "",
                Price = 10,
            };

            var shortDescriptionProduct = new Product
            {
                Name = "Test Product",
                Description = "Short",
                Price = 10,
            };

            // Act
            var exceptionEmptyDescription = await Assert.ThrowsAsync<ArgumentException>(() => productService.UpdateAsync(emptyDescriptionProduct));
            var exceptionShortDescription = await Assert.ThrowsAsync<ArgumentException>(() => productService.UpdateAsync(shortDescriptionProduct));

            // Assert
            Assert.Equal("Product description cannot be empty.", exceptionEmptyDescription.Message);
            Assert.Equal("Product description should have more than 20 characters.", exceptionShortDescription.Message);
        }
    }
}