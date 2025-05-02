using Ecommerce.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Queries.Product.Suggestion
{
    public class SuggestionSearchQueryHandler : IQueryHandler<SuggestionSearchQuery, List<string>>
    {
        private readonly IProductSearchService _productSearchService;

        public SuggestionSearchQueryHandler(IProductSearchService productSearchService)
        {
            _productSearchService = productSearchService;
        }

        public async Task<List<string>> HandleAsync(SuggestionSearchQuery query)
        {
            return await _productSearchService.SuggestionSearchAsync(query.Query);
        }
    }
}