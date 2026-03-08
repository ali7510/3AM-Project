using AutoMapper;
using Ecommerce.Domain.ProductModule;
using Ecommerce.Shared.ProductDTOs;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Service.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Category, CategoryDTO>();
            //CreateMap<Product, ProductDTO>()
            //    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            //    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Stock_Quantity))
            //    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image_Url));

            CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Stock_Quantity))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image_Url))
            .ForMember(dest => dest.SpecsJson, opt => opt.MapFrom(src =>
                string.IsNullOrWhiteSpace(src.specsJson)
                ? null
                : TryDeserialize(src.specsJson)));

            CreateMap<Product, FullProductDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Stock_Quantity))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image_Url))
            .ForMember(dest => dest.isActive, opt => opt.MapFrom(src => src.isActive))
            .ForMember(dest => dest.SpecsJson, opt => opt.MapFrom(src =>
                string.IsNullOrWhiteSpace(src.specsJson)
                ? null
                : TryDeserialize(src.specsJson)));


            //CreateMap<AddProductDTO, Product>()
            //    .ForMember(dest => dest.Image_Url, opt => opt.Ignore());

            CreateMap<AddProductDTO, Product>()
            .ForMember(dest => dest.Image_Url, opt => opt.Ignore())
            .ForMember(dest => dest.Stock_Quantity, opt => opt.MapFrom(src => src.Stock_Quantity))
            .ForMember(dest => dest.specsJson, opt => opt.MapFrom(src =>
                src.specsJson == null ? null: JsonSerializer.Serialize(src.specsJson, (JsonSerializerOptions)null)));

            //CreateMap<UpdateProductDTO, Product>()
            //    .ForMember(dest => dest.Image_Url, opt => opt.Ignore())
            //    .ForMember(dest => dest.Stock_Quantity, opt => opt.MapFrom(src => src.Stock_Quantity))
            //    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
            //    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            //    .ForMember(dest => dest.specsJson, opt => opt.MapFrom(src =>
            //        src.specsJson == null ? null : JsonSerializer.Serialize(src.specsJson, (JsonSerializerOptions)null)));
            
        }

        private static object? TryDeserialize(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<object>(json);
            }
            catch
            {
                return json;
            }
        }
    }
}
