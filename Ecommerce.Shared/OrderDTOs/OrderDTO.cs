using Ecommerce.Shared.CartDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.OrderDTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string? User_Name { get; set; }
        public decimal Total_Price { get; set; }
        public string Status { get; set; } = default!;
        public string Payment_Status { get; set; } = default!;
        public ICollection<CartItemDTO> CartItems { get; set; } = default!;
    }
}
