using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction.IAuthServices
{
    public interface IEmailService
    {
        Task SendOtpAsync(string toEmail, string otpCode);
    }
}
