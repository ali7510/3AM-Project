using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.UserModule;
using Ecommerce.ServiceAbstraction.AuthServices;
using Ecommerce.Shared.UserDTOs;
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
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDTO dto)
        {
            var message = await _authService.RegisterAsync(dto);
            return Ok(message);
        }

        [HttpPost("request-otp")]
        public async Task<ActionResult> RequestOtp([FromBody] RequestOtpDTO dto)
        {
            await _authService.RequestOtpAsync(dto);
            return Ok("OTP sent to your email.");
        }

        [HttpPost("verify-otp")]
        public async Task<ActionResult<AuthResponseDTO>> VerifyOtp([FromBody] VerifyOtpDTO dto)
        {
            var result = await _authService.VerifyOtpAsync(dto);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDTO>> RefreshToken([FromBody] RefreshTokenDTO dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _authService.RevokeRefreshTokenAsync(userId);
            return Ok("Logged out successfully.");
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<ActionResult> DeleteUser()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _authService.DeleteUser(userId);
            return Ok("User deleted successfully.");
        }
    }
}
