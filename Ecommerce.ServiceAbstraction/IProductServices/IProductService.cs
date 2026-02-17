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
        public Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        public Task<ProductDTO> GetProductByIdAsync(int id);
        public Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
    }
}
