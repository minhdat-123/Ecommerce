using Ecommerce.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Queries.Product.Search
{
    public class SearchProductsQueryHandler : IQueryHandler<SearchProductsQuery, List<Domain.ElasticSearch.Documents.ProductDocument>>
    {
        private readonly IProductSearchService _productSearchService;

        public SearchProductsQueryHandler(IProductSearchService productSearchService)
        {
            _productSearchService = productSearchService;
        }

        public async Task<List<Domain.ElasticSearch.Documents.ProductDocument>> HandleAsync(SearchProductsQuery query)
        {
            return await _productSearchService.SearchProductsAsync(query);
        }
    }

}
