using Ecommerce.Domain.CartModule;
using Ecommerce.Domain.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Contracts
{
    public interface ICartRepository
    {
        public Task<Cart?> GetUserCartsAsync(int userId);

        public Task CreateCartAsync(Cart cart);

        public Task AddToCartAsync(CartItem cartItem);

        public Task RemoveFromCartAsync(CartItem cartItem);
        public Task<CartItem?> GetCartItemByIdAsync(int id);

        public Task<int> GetCartId(int userId);

        public Task UpdateProduct(Product product);

        public Task UpdateCartItem(CartItem cartItem);


    }
}
