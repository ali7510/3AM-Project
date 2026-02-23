using AutoMapper;
using Ecommerce.Domain.CartModule;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.OrderModule;
using Ecommerce.Domain.ProductModule;
using Ecommerce.Domain.UserModule;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.CartDTOs;
using Ecommerce.Shared.OrderDTOs;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CartService(ICartRepository cartRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddToCart(int userId, AddCartItemDTO addCartItemDTO)
        {
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);
                if (user is null)
                    throw new KeyNotFoundException("User not found");

                if (user.isActive == false)
                    throw new UnauthorizedAccessException("User is inactive");

                Cart? cart = await _cartRepository.GetUserCartsAsync(userId);
                if (cart is null)
                {
                    cart = new Cart
                    {
                        User_Id = userId,
                    };
                    await _cartRepository.CreateCartAsync(cart);
                }
                var item = cart.CartItems.Where(c => c.Product_Id == addCartItemDTO.Product_Id).FirstOrDefault();
                if (item is not null)
                {
                    item.Quantity += addCartItemDTO.Quantity;
                    await _cartRepository.UpdateCartItem(item);
                    return;
                }
                var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(addCartItemDTO.Product_Id);
                if (product is null)
                    throw new KeyNotFoundException("Product not found");
                product.Stock_Quantity -= addCartItemDTO.Quantity;
                await _cartRepository.UpdateProduct(product);
                var cartItem = _mapper.Map<CartItem>(addCartItemDTO);
                cartItem.Cart_Id = cart.Id;
                await _cartRepository.AddToCartAsync(cartItem);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }

        }

        public async Task<OrderDTO> CheckOut(int userId)
        {
            try
            {
                var cart = await _cartRepository.GetUserCartsAsync(userId);
                if (cart is null)
                    throw new KeyNotFoundException("Cart not found");

                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);
                if (user is null)
                    throw new KeyNotFoundException("User not found");
                if (user.isActive == false)
                    throw new UnauthorizedAccessException("User is inactive");

                var OrderDTO = _mapper.Map<OrderDTO>(cart);
                return OrderDTO;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task ClearCart(int userId)
        {
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);
                if (user is null)
                    throw new KeyNotFoundException("User not found");

                if (user.isActive == false)
                    throw new UnauthorizedAccessException("User is inactive");

                var cart = await _cartRepository.GetUserCartsAsync(userId);
                if (cart is null)
                    throw new KeyNotFoundException("Cart not found");
                foreach (var item in cart.CartItems.ToList())
                {
                    if (item.Product != null)
                        item.Product.Stock_Quantity += item.Quantity;
                    await _cartRepository.RemoveFromCartAsync(item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<CartDTO> GetUserCart(int id)
        {
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
                if (user is null)
                    throw new KeyNotFoundException("User not found");

                if (user.isActive == false)
                    throw new UnauthorizedAccessException("User is inactive");

                var carts = await _cartRepository.GetUserCartsAsync(id);
                return _mapper.Map<CartDTO>(carts);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task RemoveItemFromCart(int userId, int cartItemId)
        {
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);
                if (user is null)
                    throw new KeyNotFoundException("User not found");

                if (user.isActive == false)
                    throw new UnauthorizedAccessException("User is inactive");

                var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);

                if (cartItem is null)
                    throw new KeyNotFoundException("Cart item not found");

                if (cartItem.Cart.User_Id != userId)
                    throw new UnauthorizedAccessException();

                if (cartItem.Product != null)
                    cartItem.Product.Stock_Quantity += cartItem.Quantity;

                await _cartRepository.RemoveFromCartAsync(cartItem);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");

            }
        }
    }
}
