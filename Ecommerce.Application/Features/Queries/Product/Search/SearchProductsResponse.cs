using Ecommerce.Domain.ElasticSearch.Documents;
using System.Collections.Generic;

namespace Ecommerce.Application.Features.Queries.Product.Search
{
    public class SearchProductsResponse
    {
        public List<ProductDocument> Products { get; set; }
        public long TotalCount { get; set; }

        public SearchProductsResponse(List<ProductDocument> products, long totalCount)
        {
            Products = products;
            TotalCount = totalCount;
        }
    }
}