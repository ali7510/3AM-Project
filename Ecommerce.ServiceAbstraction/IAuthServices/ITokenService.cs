using Ecommerce.Domain.UserModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction.IAuthServices
{
    public interface ITokenService
    {
        public string GenerateToken(User user);

    }
}
