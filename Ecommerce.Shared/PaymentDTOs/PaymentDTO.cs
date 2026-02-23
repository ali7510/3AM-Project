using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.PaymentDTOs
{
    public class PaymentDTO
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? PaymentUrl { get; set; }  //MyFatoorah

        public string? ExternalPaymentId { get; set; } // InvoiceId

        public bool RequiresRedirect { get; set; }
    }
}
