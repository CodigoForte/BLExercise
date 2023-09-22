using BL.Core.Domain;
using BL.Data;
using BL.Services;
using Moq;

namespace BL.Tests.Products
{
    public class AddProductTests
    {
        private readonly Mock<IProductRepository> mockProductRepo;
        private readonly IProductService productService;

        public AddProductTests()
        {
            mockProductRepo = new Mock<IProductRepository>();
            productService = new ProductService(mockProductRepo.Object);
        }

        [Fact]
        public async Task Price_Should_Be_Greater_Than_Zero_On_AddProduct()
        {
            // Arrange
            var productWithNegativePrice = new Product
            {
                Name = "Test Product",
                Description = "Test Description for the product",
                Price = -1,
            };

            var productWithPrice0 = new Product
            {
                Name = "Test Product",
                Description = "Test Description for the product",
                Price = 0,
            };

            // Act
            var exceptionWithNegativePrice = await Assert.ThrowsAsync<ArgumentException>(() => productService.AddAsync(productWithNegativePrice));
            var exceptionWithPrice0 = await Assert.ThrowsAsync<ArgumentException>(() => productService.AddAsync(productWithPrice0));

            // Assert
            Assert.Equal("The product should have a price above zero.", exceptionWithNegativePrice.Message);
            Assert.Equal("The product should have a price above zero.", exceptionWithPrice0.Message);
        }

        [Fact]
        public async Task Price_Should_Not_Be_Greater_Than_10000_On_AddProduct()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Description = "Test Description for the product",
                Price = 10001,
            };

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => productService.AddAsync(product));

            // Assert
            Assert.Equal("The product's price shouldn't exceed 10,000.", exception.Message);
        }

        [Fact]
        public async Task Product_Name_Should_Not_Be_Empty_On_AddProduct()
        {
            // Arrange
            var product = new Product
            {
                Name = "",
                Description = "Test Description for the product",
                Price = 10,
            };

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => productService.AddAsync(product));

            // Assert
            Assert.Equal("Product name cannot be empty.", exception.Message);
        }

        [Fact]
        public async Task Product_Name_Should_Be_Between_2_And_50_Characters_On_AddProduct()
        {
            // Arrange
            var shortNameProduct = new Product
            {
                Name = "A",
                Description = "Test Description for the product",
                Price = 10,
            };

            var longNameProduct = new Product
            {
                Name = new string('A', 51),
                Description = "Test Description for the product",
                Price = 10,
            };

            var correctNameProduct = new Product
            {
                Name = "Test Product",
                Description = "Test Description for the product",
                Price = 10,
            };

            mockProductRepo.Setup(repo => repo.InsertAsync(correctNameProduct)).ReturnsAsync(
                new Product()
                { 
                    Id = 1,
                    Name = correctNameProduct.Name,
                    Description = correctNameProduct.Description,
                    Price = correctNameProduct.Price
                }
            );

            // Act
            var exceptionShortName = await Assert.ThrowsAsync<ArgumentException>(() => productService.AddAsync(shortNameProduct));
            var exceptionLongName = await Assert.ThrowsAsync<ArgumentException>(() => productService.AddAsync(longNameProduct));
            var addedProduct = await productService.AddAsync(correctNameProduct);

            // Assert
            Assert.Equal("Product name should have between 2 and 50 characters.", exceptionShortName.Message);
            Assert.Equal("Product name should have between 2 and 50 characters.", exceptionLongName.Message);
            Assert.True(addedProduct.Id > 0);
        }

        [Fact]
        public async Task Product_Description_Should_Not_Be_Empty_And_Have_Min_20_Characters_On_AddProduct()
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
            var exceptionEmptyDescription = await Assert.ThrowsAsync<ArgumentException>(() => productService.AddAsync(emptyDescriptionProduct));
            var exceptionShortDescription = await Assert.ThrowsAsync<ArgumentException>(() => productService.AddAsync(shortDescriptionProduct));

            // Assert
            Assert.Equal("Product description cannot be empty.", exceptionEmptyDescription.Message);
            Assert.Equal("Product description should have more than 20 characters.", exceptionShortDescription.Message);
        }
    }
}