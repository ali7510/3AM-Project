using Ecommerce.Domain.OrderModule;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.PaymentServices
{
    public class MyFatoorahSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string CallbackUrl { get; set; } = string.Empty;
        public string ErrorUrl { get; set; } = string.Empty;

    }
}
