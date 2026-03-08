using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.ProductDTOs
{
    public class UpdateProductDTO
    {
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int? Stock_Quantity { get; set; }

    }
}
