using Ecommerce.Shared.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.ProfileDTOs
{
    public class ProfileDTO
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public ICollection<OrderDTO> Orders { get; set; } = new List<OrderDTO>();
    }
}
