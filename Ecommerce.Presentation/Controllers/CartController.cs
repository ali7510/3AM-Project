using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.CartDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _serviceCart;

        public CartController(ICartService serviceCart)
        {
            _serviceCart = serviceCart;
        }

        [HttpGet("{id}")]
        // Get: BaseURL/api/carts/id
        public async Task<ActionResult<CartDTO>> GetUserCart(int id)
        {
            var carts = await _serviceCart.GetUserCart(id);
            return Ok(carts);
        }

        [HttpPost("items/{userId}")]
        // Post: BaseURL/api/carts/userId
        public async Task<ActionResult> AddToCart(int userId, [FromBody] AddCartItemDTO dto)
        {
            await _serviceCart.AddToCart(userId, dto);
            return Ok();
        }

        [HttpDelete("items/{userId}/{cartItemId}")]
        // Delete: BaseURL/api/carts/cartItemId
        public async Task<ActionResult> RemoveItemFromCart(int userId, int cartItemId)
        {
            await _serviceCart.RemoveItemFromCart(userId, cartItemId);
            return Ok();
        }

        [HttpDelete("/items/clear/{userId}")]
        public async Task<ActionResult> ClearCart(int userId)
        {
            await _serviceCart.ClearCart(userId);
            return Ok();
        }
    }
}
