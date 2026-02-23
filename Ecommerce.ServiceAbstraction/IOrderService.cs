using Ecommerce.Domain.CartModule;
using Ecommerce.Shared.CartDTOs;
using Ecommerce.Shared.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction
{
    public interface IOrderService
    {
        public Task<OrderDTO> ViewCurrentOrder(int userId);
    }
}
