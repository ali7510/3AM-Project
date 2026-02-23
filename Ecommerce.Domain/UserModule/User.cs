using Ecommerce.Domain.CartModule;
using Ecommerce.Domain.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.UserModule
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool isActive { get; set; } = true;
        public UserRoles Role { get; set; } = UserRoles.Customer;

        #region Relationships
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public Cart Cart { get; set; } = default!;

        public string? OtpCode { get; set; }
        public DateTime? OtpExpiry { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        #endregion
    }
}