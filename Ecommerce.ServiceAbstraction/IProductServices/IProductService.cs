using Ecommerce.Shared.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction.IProductServices
{
    public interface IProductService
    {
        public Task<IEnumerable<FullProductDTO>> GetAllProductsAsync();
        public Task<IEnumerable<ProductDTO>> GetAllActiveProductsAsync();
        public Task<ProductDTO> GetProductByIdAsync(int id, int? w, int? h);
        public Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        public Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(int categoryId);
        public Task<IEnumerable<ProductDTO>> GetAllVehicles();
        public Task<IEnumerable<ProductDTO>> GetAllAccessories();
        public Task<ProductDTO> AddProductAsync(AddProductDTO productDTO);
        public Task<ProductDTO> UpdateProductAsync(int id, UpdateProductDTO productDTO);
        public Task<bool> ToggleProductAsync(int id);
    }
}
