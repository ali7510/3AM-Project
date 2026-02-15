using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.ProductModule
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = default!;
        public int? Parent_Category_Id { get; set; }
        public Category? Parent_Category { get; set; }
        public ICollection<Category> Sub_Categories { get; set; } = new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
