using AutoMapper;
using Ecommerce.Domain.ProductModule;
using Ecommerce.Shared.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Stock_Quantity))
                .ForMember(dest=>dest.ImageUrl, opt => opt.MapFrom(src => src.Image_Url));
        }
    }
}
