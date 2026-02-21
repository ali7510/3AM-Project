using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.CartDTOs;
using Ecommerce.Shared.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, ICartRepository cartRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public Task ConfirmOrder(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDTO> ViewCurrentOrder(int userId)
        {
            var cart = await _cartRepository.GetUserCartsAsync(userId);
            if (cart is null)
                throw new KeyNotFoundException("Cart not found");
            var OrderDTO = _mapper.Map<OrderDTO>(cart);
            return OrderDTO;
        }
    }
}
