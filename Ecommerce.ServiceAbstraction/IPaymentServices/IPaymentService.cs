using Ecommerce.Domain.OrderModule;
using Ecommerce.Shared.OrderDTOs;
using Ecommerce.Shared.PaymentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction.IPaymentServices
{
    public interface IPaymentService
    {
        Task<PaymentResponseDTO> ConfirmPaymentAsync(int userId, PaymentMethod method, string frontendUrl);
        Task<int> HandleCallbackAsync(string invoiceId);
    }
}
