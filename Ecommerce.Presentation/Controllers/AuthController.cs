using Ecommerce.ServiceAbstraction.AuthServices;
using Ecommerce.Shared.UserDTOs;
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
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        // Post: BaseURL/api/auth/register
        public async Task<ActionResult> Register([FromBody] RegisterDTO dto)
        {
            await _authService.RegisterAsync(dto);
            return Ok("User Registered Seccessfully!");
        }

        [HttpPost("login")]
        // Post: BaseURL/api/auth/login
        public async Task<ActionResult<string>> Login([FromBody] LoginDTO dto)
        {
            var token = await _authService.LoginAsync(dto);
            return Ok(token);
        }
    }
}
