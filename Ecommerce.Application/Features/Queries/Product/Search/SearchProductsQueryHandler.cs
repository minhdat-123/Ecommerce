using Ecommerce.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Queries.Product.Search
{
    public class SearchProductsQueryHandler : IQueryHandler<SearchProductsQuery, SearchProductsResponse>
    {
        private readonly IProductSearchService _productSearchService;

        public SearchProductsQueryHandler(IProductSearchService productSearchService)
        {
            _productSearchService = productSearchService;
        }

        public async Task<SearchProductsResponse> HandleAsync(SearchProductsQuery query)
        {
            var (products, totalCount) = await _productSearchService.SearchProductsAsync(query);
            return new SearchProductsResponse(products, totalCount);
        }
    }

}
