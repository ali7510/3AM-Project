using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.CartDTOs
{
    public class CartDTO
    {
        public int Id { get; set; }
        public string User_Name { get; set; } = string.Empty;
        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();
    }
}
