using AutoMapper;
using Ecommerce.Domain.CartModule;
using Ecommerce.Domain.OrderModule;
using Ecommerce.Shared.CartDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.MappingProfiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.User_Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems.ToList()));

            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Product_Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Product_Price, opt => opt.MapFrom(src => src.Product.Price)).ReverseMap();

            CreateMap<AddCartItemDTO, CartItem>();

        }
    }
}
