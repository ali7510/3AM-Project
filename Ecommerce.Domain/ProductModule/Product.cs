using Ecommerce.Domain.CartModule;
using Ecommerce.Domain.OrderModule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.ProductModule
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int Stock_Quantity { get; set; }
        public string Image_Url { get; set; } = default!;
        public string Brand { get; set; } = default!;
        public string? specsJson { get; set; }

        #region Relationships
        public int Category_Id { get; set; }
        public Category Category { get; set; } = default!;

        public ICollection<OrderItem> orderItems { get; set; } = new List<OrderItem>();

        public ICollection<CartItem> cartItems { get; set; } = new List<CartItem>();
        #endregion
    }
}
