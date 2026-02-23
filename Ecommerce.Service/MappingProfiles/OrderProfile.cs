using AutoMapper;
using Ecommerce.Domain.CartModule;
using Ecommerce.Domain.OrderModule;
using Ecommerce.Shared.CartDTOs;
using Ecommerce.Shared.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.MappingProfiles
{
    public class OrderProfile : Profile
    {

        public OrderProfile()
        {
            CreateMap<Cart, OrderDTO>()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems))
                .ForMember(dest => dest.Total_Price, opt => opt.MapFrom(src => src.CartItems.Sum(item => item.Quantity * item.Product.Price)))
                .ForMember(dest => dest.User_Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => OrderStatus.PendingPayment))
                .ForMember(dest => dest.Payment_Status, opt => opt.MapFrom(src => PaymentStatus.Pending))
                .ForMember(dest => dest.User_Id, opt=>opt.MapFrom(src=>src.User_Id));

            CreateMap<OrderDTO, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.CartItems))
                // Convert strings to Enums (Case-insensitive)
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<OrderStatus>(src.Status, true)))
                .ForMember(dest => dest.Payment_Status, opt => opt.MapFrom(src => Enum.Parse<PaymentStatus>(src.Payment_Status, true))).ReverseMap();

            // 2. Map CartItemDTO to OrderItem
            CreateMap<CartItemDTO, OrderItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Product_Id, opt => opt.MapFrom(src => src.Product_Id)) // Usually Id in CartItemDTO refers to ProductId
                .ForMember(dest => dest.Price_At_Purchase, opt => opt.MapFrom(src => src.Product_Price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                // Ignore the navigation properties to avoid circular reference issues during mapping
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());

        }
    }
}
