using Ecommerce.ServiceAbstraction.IProfileServices;
using Ecommerce.Shared.ProfileDTOs;
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
    public class AccountController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public AccountController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteUser()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _profileService.DeleteUser(userId);
            return Ok("User deleted successfully.");
        }

        [HttpGet]
        public async Task<ActionResult<ProfileDTO>> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var profile = await _profileService.GetUserProfile(userId);
            return Ok(profile);
        }
    }
}
