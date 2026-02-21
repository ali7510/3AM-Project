using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.CartDTOs;
using Ecommerce.Shared.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _serviceCart;

        public CartController(ICartService serviceCart)
        {
            _serviceCart = serviceCart;
        }

        [HttpGet("mycart")]
        // Get: BaseURL/api/carts/mycart
        public async Task<ActionResult<CartDTO>> GetUserCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool isParsed = int.TryParse(userId, out int id);
            var carts = await _serviceCart.GetUserCart(id);
            return Ok(carts);
        }

        [HttpPost("items")]
        // Post: BaseURL/api/carts/userId
        public async Task<ActionResult> AddToCart([FromBody] AddCartItemDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool isParsed = int.TryParse(userId, out int id);
            await _serviceCart.AddToCart(id, dto);
            return Ok();
        }

        [HttpDelete("items/{cartItemId}")]
        // Delete: BaseURL/api/carts/cartItemId
        public async Task<ActionResult> RemoveItemFromCart(int cartItemId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool isParsed = int.TryParse(userId, out int id);
            await _serviceCart.RemoveItemFromCart(id, cartItemId);
            return Ok();
        }

        [HttpDelete("/items/clear")]
        public async Task<ActionResult> ClearCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool isParsed = int.TryParse(userId, out int id);
            await _serviceCart.ClearCart(id);
            return Ok();
        }

        [HttpGet("checkout")]
        // Post: BaseURL/api/carts/checkout
        public async Task<ActionResult<OrderDTO>> CheckOut()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool isParsed = int.TryParse(userId, out int id);
            var order = await _serviceCart.CheckOut(id);
            return Ok(order);
        }
    }
}
