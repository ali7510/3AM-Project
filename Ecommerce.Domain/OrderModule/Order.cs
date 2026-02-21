using Ecommerce.Domain.CartModule;
using Ecommerce.Domain.UserModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.OrderModule
{
    public class Order : BaseEntity
    {
        public OrderStatus Status { get; set; }
        public decimal Total_Price { get; set; }
        public PaymentStatus Payment_Status { get; set; } = PaymentStatus.Pending;

        #region Relationships
        public int User_Id { get; set; }
        public User User { get; set; } = default!;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Payment Payment { get; set; } = default!;
        #endregion
    }
}
