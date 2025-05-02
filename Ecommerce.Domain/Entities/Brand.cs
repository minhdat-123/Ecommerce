using System.Collections.Generic;

namespace Ecommerce.Domain.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<BrandCategory> BrandCategories { get; set; }
    }
}