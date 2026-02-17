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
        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var Categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(Categories);

        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var Products = await _unitOfWork.GetRepository<Product>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(Products);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var Product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id);
            return _mapper.Map<ProductDTO>(Product);
        }
    }
}
