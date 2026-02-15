using Ecommerce.Domain.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.CartModule
{
    public class CartItem : BaseEntity
    {
        public int Quantity { get; set; } = default!;

        #region Relationships
        public int Cart_Id { get; set; }
        public Cart Cart { get; set; } = default!;

        public int Product_Id { get; set; }
        public Product Product { get; set; } = default!;
        #endregion
    }
}
