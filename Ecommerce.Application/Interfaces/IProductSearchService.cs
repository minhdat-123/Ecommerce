using ProductService.Application.Features.Queries.Product.Search;
using ProductService.Domain.ElasticSearch.Documents;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Interfaces
{
    public interface IProductSearchService
    {
        Task<List<ProductDocument>> SearchProductsAsync(string query); // Keep for backward compatibility
        Task<(List<ProductDocument> Products, long TotalCount)> SearchProductsAsync(SearchProductsQuery query);
        Task<List<string>> SuggestionSearchAsync(string query);
        Task IndexProductAsync(ProductDocument product);
        Task DeleteProductAsync(int id);
    }

}

