using Ecommerce.ServiceAbstraction;
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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("vieworder")]
        // Get: BaseURL/api/order/userId
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<OrderDTO>> ViewCurrentOrder()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool isParsed = int.TryParse(userId, out int id);
            var order = await _orderService.ViewCurrentOrder(id);
            return Ok(order);
        }
    }
}
