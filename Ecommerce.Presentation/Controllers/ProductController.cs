using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.ServiceAbstraction.IProductServices;
using Ecommerce.Shared.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Ecommerce.Domain.UserModule;


namespace Ecommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        // Get: BaseURL/api/products
        public async Task<ActionResult<IEnumerable<FullProductDTO>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }


        [HttpGet("available-products")]
        // Get: BaseURL/api/products
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetActiveProducts()
        {
            var products = await _productService.GetAllActiveProductsAsync();
            return Ok(products);
        }


        [HttpPost("add-product")]
        [Authorize(Roles = "Admin")]
        // Post: BaseURL/api/products
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromForm] AddProductDTO productCreateDTO)
        {
            var createdProduct = await _productService.AddProductAsync(productCreateDTO);

            return CreatedAtAction(
                nameof(GetProduct), 
                new { id = createdProduct.Id },
                createdProduct
            );
        }

        [HttpPut("update-product/{id}")]
        [Authorize(Roles = "Admin")]
        // Put: BaseURL/api/products/update-product/id
        public async Task<ActionResult<ProductDTO>> UpdateProduct(int id, [FromForm] UpdateProductDTO productUpdateDTO)
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, productUpdateDTO);
            return Ok(updatedProduct);
        }

        [HttpDelete("toggle-product/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ToggleProduct(int id)
        {
            var success = await _productService.ToggleProductAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }

        [HttpGet("{id}")]
        // Get: BaseURL/api/products/id
        public async Task<ActionResult<ProductDTO>> GetProduct(int id, [FromQuery] int? w, [FromQuery] int? h)
        {
            var product = await _productService.GetProductByIdAsync(id, w, h);
            return Ok(product);
        }

        [HttpGet("categories")]
        // Get: BaseURL/api/products/categories
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            var categories = await _productService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("category/{categoryId}")]
        // Get: BaseURL/api/products/category/categoryId
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }

        [HttpGet("vehicles")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllVehicles()
        {
            var vehicles = await _productService.GetAllVehicles();
            return Ok(vehicles);
        }

        [HttpGet("accessories")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllAccessories()
        {
            var accessories = await _productService.GetAllAccessories();
            return Ok(accessories);
        }
    }
}
