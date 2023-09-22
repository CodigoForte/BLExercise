using BL.Core.Domain;
using BL.Services;
using BL.WebAPI.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BL.WebAPI.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            GetProductsResult result = new();

            try
            {
                var products = await _productService.GetAllAsync();
                
                result.IsSuccess = true;
                result.Products = products;

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;

                return BadRequest(result);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            GetProductResult result = new();

            try
            {
                var product = await _productService.GetByIdAsync(id);

                result.IsSuccess = true;
                result.Product = product;

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;

                return NotFound(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;

                return BadRequest(result);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(NewProduct newProduct)
        {
            AddProductResult result = new();

            try
            {
                var product = new Product()
                {
                    Name = newProduct.Name,
                    Description = newProduct.Description,
                    Price = newProduct.Price,
                };

                var addedProduct = await _productService.AddAsync(product);

                result.IsSuccess = true;
                result.Product = addedProduct;

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;

                return BadRequest(result);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Product product)
        {
            UpdateProductResult result = new();

            try
            {
                var updatedProduct = await _productService.UpdateAsync(product);

                result.IsSuccess = true;
                result.Product = updatedProduct;

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;

                return NotFound(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;

                return BadRequest(result);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            DeleteProductResult result = new();

            try
            {
                await _productService.RemoveAsync(id);

                result.IsSuccess = true;

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;

                return NotFound(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;

                return BadRequest(result);
            }
        }
    }
}