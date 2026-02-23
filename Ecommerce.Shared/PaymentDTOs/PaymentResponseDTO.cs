using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.PaymentDTOs
{
    public class PaymentResponseDTO
    {
        public bool Success { get; set; }
        public string? PaymentUrl { get; set; }
        public string? ExternalPaymentId { get; set; }
        public bool RequiresRedirect { get; set; }
        public string? Message { get; set; }
    }
}
