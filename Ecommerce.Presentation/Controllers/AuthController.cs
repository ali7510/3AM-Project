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
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IAuthService authService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
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
        public async Task<ActionResult> Login([FromBody] LoginDTO dto)
        {
            var token = await _authService.LoginAsync(dto);
            return Ok(new { token });
        }
        //public async Task<ActionResult> Login([FromBody] LoginDTO dto)
        //{
        //    try
        //    {
        //        var token = await _authService.LoginAsync(dto);
        //        return Ok(new { token });
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //}

        [HttpDelete("delete")]
        // Delete: BaseURL/api/auth/delete/1
        public async Task<ActionResult> DeleteUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool isParsed = int.TryParse(userId, out int id);
            var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            if (user.isActive == false)
            {
                return BadRequest("User is already inactive");
            }
            await _authService.DeleteUser(id);
            return Ok("User Deleted Seccessfully!");
        }
    }
}
