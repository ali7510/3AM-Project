using Ecommerce.Shared.CartDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction
{
    public interface ICartService
    {
        public Task<CartDTO> GetUserCart(int id);

        public Task AddToCart(int userId, AddCartItemDTO addCartItemDTO);

        public Task RemoveItemFromCart(int userId, int cartItemId);

        public Task ClearCart(int userId);


    }
}
