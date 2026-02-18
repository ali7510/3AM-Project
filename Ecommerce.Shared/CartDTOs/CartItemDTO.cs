using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.CartDTOs
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public string? Product_Name { get; set; }
        public decimal Product_Price { get; set; }
        public int Quantity { get; set; }
    }
}
