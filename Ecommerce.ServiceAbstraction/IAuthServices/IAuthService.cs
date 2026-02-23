
using Ecommerce.Shared.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction.AuthServices
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDTO dto);
        Task RequestOtpAsync(RequestOtpDTO dto);
        Task<AuthResponseDTO> VerifyOtpAsync(VerifyOtpDTO dto);
        Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenDTO dto);
        Task RevokeRefreshTokenAsync(int userId);
    }
}
