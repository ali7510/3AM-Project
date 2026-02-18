using Ecommerce.Domain.CartModule;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.ProductModule;
using Ecommerce.Persistence.Data.DBcontexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly StoreDbContext _dbContext;

        public CartRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddToCartAsync(CartItem cartItem)
        {
            _dbContext.CartItems.Add(cartItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateCartAsync(Cart cart)
        {
            await _dbContext.Carts.AddAsync(cart);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CartItem?> GetCartItemByIdAsync(int id)
        {
            return await _dbContext.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.Id == id);

        }

        public async Task<int> GetCartId(int userId)
        {
            int cartId = await _dbContext.Carts.Where(c => c.User_Id == userId).Select(c => c.Id).FirstOrDefaultAsync();
            return cartId;
        }

        public async Task<Cart?> GetUserCartsAsync(int userId)
        {
            return await _dbContext.Carts
            .Include(c => c.User)
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .Where(c => c.User_Id == userId).FirstOrDefaultAsync();
        }

        public async Task RemoveFromCartAsync(CartItem cartItem)
        {
            _dbContext.CartItems.Remove(cartItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCartItem(CartItem cartItem)
        {
            _dbContext.CartItems.Update(cartItem);
            await _dbContext.SaveChangesAsync();
        }
    }
}
