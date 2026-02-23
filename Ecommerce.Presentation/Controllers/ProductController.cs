using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.ServiceAbstraction.IProductServices;
using Ecommerce.Shared.ProductDTOs;
using Microsoft.AspNetCore.Authorization;


namespace Ecommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        // Get: BaseURL/api/products
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        // Get: BaseURL/api/products/id
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
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
