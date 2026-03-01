using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.ProductModule;
using Ecommerce.ServiceAbstraction.ICloudinaryServices;
using Ecommerce.ServiceAbstraction.IProductServices;
using Ecommerce.Shared.ProductDTOs;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Service.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ProductDTO> AddProductAsync(AddProductDTO productDTO)
        {
            if (productDTO == null)
                throw new ArgumentNullException(nameof(productDTO));

            if(productDTO.Category_Id == 1 || productDTO.Category_Id == 2)
                throw new ArgumentException("Invalid category ID. Category ID cannot be 1 or 2.");

            var product = _mapper.Map<Product>(productDTO);
            product.Image_Url = await _cloudinaryService.UploadImageAsync(productDTO.ImageFile);
            await _unitOfWork.GetRepository<Product>().AddAsync(product);
            await _unitOfWork.SaveChanges();
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAccessories()
        {
            var accessories = await _unitOfWork.GetRepository<Product>().GetAllAsync(default, p => p.Category);
            var items = accessories.Where(a => a.Category?.Parent_Category_Id == 2);
            return _mapper.Map<IEnumerable<ProductDTO>>(items);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var Categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(Categories);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var Products = await _unitOfWork.GetRepository<Product>().GetAllAsync(default,p=>p.Category);
            return _mapper.Map<IEnumerable<ProductDTO>>(Products);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllVehicles()
        {
            var cars = await _unitOfWork.GetRepository<Product>().GetAllAsync(default ,p => p.Category);
            var vehicles = cars.Where(c => c.Category?.Parent_Category_Id == 1);
            return _mapper.Map<IEnumerable<ProductDTO>>(vehicles);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id, int? w, int? h)
        {
            var Product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id, p=>p.Category);
            if (Product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            var productDTO = _mapper.Map<ProductDTO>(Product);
            productDTO.ImageUrl = _cloudinaryService.CustomImgUrl(Product.Image_Url, w, h);
            return _mapper.Map<ProductDTO>(productDTO);
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _unitOfWork.GetRepository<Product>().GetAllAsync(p=>p.Category_Id == categoryId, p=>p.Category);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }
    }
}
