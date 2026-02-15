using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.OrderModule
{
    public class Payment : BaseEntity
    {
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }

        #region Relationships
        public int Order_Id { get; set; }
        public Order Order { get; set; } = default!;
        #endregion
    }
}
