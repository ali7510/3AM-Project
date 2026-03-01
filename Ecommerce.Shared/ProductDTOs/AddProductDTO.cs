using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.ProductDTOs
{
    public class AddProductDTO
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int Stock_Quantity { get; set; }
        public IFormFile ImageFile { get; set; } = default!;
        public string Brand { get; set; } = default!;
        public object? specsJson { get; set; }
        public int Category_Id { get; set; }
    }
}
