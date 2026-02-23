using Ecommerce.Domain.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.PaymentDTOs
{
    public class ConfirmPaymentDTO
    {
        public PaymentMethod Method { get; set; }
    }
}
