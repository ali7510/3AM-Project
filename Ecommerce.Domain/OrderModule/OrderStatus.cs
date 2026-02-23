using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.OrderModule
{
    public enum OrderStatus
    {
        PendingPayment = 1,
        Processing = 2,
        Shipped = 3,
        Cancelled = 4
    }
}
