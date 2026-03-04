using Ecommerce.Service.PaymentServices;
using Ecommerce.ServiceAbstraction.IPaymentServices;
using Ecommerce.Shared.PaymentDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;


namespace Ecommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/payment")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOptions<MyFatoorahSettings> _settings;

        public PaymentController(IPaymentService paymentService, IOptions<MyFatoorahSettings> settings)
        {
            _paymentService = paymentService;
            _settings = settings;
        }

        [HttpPost("confirm")]
        public async Task<ActionResult<PaymentResponseDTO>> ConfirmPayment([FromBody] ConfirmPaymentDTO dto)
        {
            // Get userId from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim);

            var result = await _paymentService.ConfirmPaymentAsync(userId, dto.Method, dto.FrontendUrl);
            return Ok(result);
        }

        [HttpGet("callback")]
        [AllowAnonymous] // MyFatoorah calls this, no JWT
        public async Task<IActionResult> Callback([FromQuery] string? paymentId, [FromQuery] string? Id, [FromQuery] string frontendUrl)
        {

            var actualPaymentId = Request.Query["paymentId"].FirstOrDefault()
                   ?? Request.Query["amp;paymentId"].FirstOrDefault()
                   ?? Request.Query["Id"].FirstOrDefault()
                   ?? Request.Query["amp;Id"].FirstOrDefault();

            try
            {
                var orderId = await _paymentService.HandleCallbackAsync(actualPaymentId!);
                return Redirect($"{frontendUrl}?payment=success&orderId={orderId}&paymentId={actualPaymentId}");
            }
            catch (Exception ex)
            {
                // Log the error (not implemented here)
                return Redirect($"{frontendUrl}?payment=failed&error={Uri.EscapeDataString(ex.Message)}");
            }
        }


        [HttpGet("status")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStatus([FromQuery] string paymentId)
        {
            try
            {
                var status = await _paymentService.HandleCallbackAsync(paymentId);
                return Ok(new { status });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
