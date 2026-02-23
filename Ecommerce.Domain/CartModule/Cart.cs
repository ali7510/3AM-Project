
using Ecommerce.Domain.UserModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.CartModule
{
    public class Cart : BaseEntity
    {
        #region Relationships
        public int User_Id { get; set; }
        public User User { get; set; } = default!;
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        #endregion
    }
}
