using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.ProductModule;
using Ecommerce.ServiceAbstraction.IProductServices;
using Ecommerce.Shared.ProductDTOs;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var Product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id, p=>p.Category);
            return _mapper.Map<ProductDTO>(Product);
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _unitOfWork.GetRepository<Product>().GetAllAsync(p=>p.Category_Id == categoryId, p=>p.Category);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        
    }
}
