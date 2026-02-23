using Ecommerce.ServiceAbstraction.IPaymentServices;
using Ecommerce.Shared.PaymentDTOs;
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
    [Route("api/payment")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("confirm")]
        public async Task<ActionResult<PaymentResponseDTO>> ConfirmPayment([FromBody] ConfirmPaymentDTO dto)
        {
            // Get userId from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim);

            var result = await _paymentService.ConfirmPaymentAsync(userId, dto.Method);
            return Ok(result);
        }

        [HttpPost("callback")]
        [AllowAnonymous] // MyFatoorah calls this, no JWT
        public async Task<IActionResult> Callback([FromBody] CallbackDTO dto)
        {
            await _paymentService.HandleCallbackAsync(dto.InvoiceId);
            return Ok();
        }
    }
}
