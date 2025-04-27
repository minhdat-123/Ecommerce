using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> Subcategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<BrandCategory> BrandCategories { get; set; }
    }

}

