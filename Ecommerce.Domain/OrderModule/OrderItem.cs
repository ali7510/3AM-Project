using Ecommerce.Domain.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.OrderModule
{
    public class OrderItem : BaseEntity
    {
        public int Quantity { get; set; }
        public decimal Price_At_Purchase { get; set; }

        #region Relationships
        public int Order_Id { get; set; }
        public Order Order { get; set; } = default!;

        public int Product_Id { get; set; }
        public Product Product { get; set; } = default!;
        #endregion
    }
}
