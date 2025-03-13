using Ecommerce.Application.Features.Queries.Product.Search;
using Ecommerce.Domain.ElasticSearch.Documents;
using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface IProductSearchService
    {
        Task<List<Product>> SearchProductsAsync(string query); // Keep for backward compatibility
        Task<List<Product>> SearchProductsAsync(SearchProductsQuery query); // New overload
        Task IndexProductAsync(ProductDocument product);
        Task DeleteProductAsync(int id);
    }

}
