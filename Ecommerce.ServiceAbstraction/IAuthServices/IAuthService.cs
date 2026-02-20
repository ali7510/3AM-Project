using Ecommerce.Domain.UserModule;
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
        public Task<string> RegisterAsync(RegisterDTO dto);
        public Task<string> LoginAsync(LoginDTO dto);
    }
}
