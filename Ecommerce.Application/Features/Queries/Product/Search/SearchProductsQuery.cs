using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Queries.Product.Search
{
    public class SearchProductsQuery
    {
        public string Query { get; set; }
        public decimal? MinPrice { get; set; } // Filter: Minimum price
        public decimal? MaxPrice { get; set; } // Filter: Maximum price
        public int? CategoryId { get; set; } // Filter: Category
        public int? ParentCategoryId { get; set; }
        public int? BrandId { get; set; }
        public string SortBy { get; set; } // Sort: "price-asc", "price-desc", "name-asc"
        public int Page { get; set; } = 1; // Pagination: Page number
        public int PageSize { get; set; } = 10; // Pagination: Items per page

    }

}
