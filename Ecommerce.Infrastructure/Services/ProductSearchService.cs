using Ecommerce.Application.Features.Queries.Product.Search;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.ElasticSearch.Documents;
using Ecommerce.Domain.Entities;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services
{
    public class ProductSearchService : IProductSearchService
    {
        private readonly ElasticsearchClient _elasticClient;

        public ProductSearchService(ElasticsearchClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<List<Product>> SearchProductsAsync(string query)
        {
            return await SearchProductsAsync(new SearchProductsQuery { Query = query });
        }

        public async Task<List<Product>> SearchProductsAsync(SearchProductsQuery query)
        {
            // Create a SearchRequest object instead of descriptor
            var searchRequest = new SearchRequest<ProductDocument>("products")
            {
                From = (query.Page - 1) * query.PageSize,
                Size = query.PageSize
            };

            // Build the query
            var boolQuery = new BoolQuery();

            // Add must clause for text search
            if (!string.IsNullOrEmpty(query.Query))
            {
                boolQuery.Must = new Query[]
                {
            new MultiMatchQuery()
            {
                Fields = new[] { "name", "description" },
                Query = query.Query,
                Fuzziness = new Fuzziness("AUTO")
            }
                };
            }

            // Add filter clauses
            var filterQueries = new List<Query>();

            // Price range filter
            if (query.MinPrice.HasValue || query.MaxPrice.HasValue)
            {
                var rangeQuery = new NumberRangeQuery("price");
                if (query.MinPrice.HasValue)
                    rangeQuery.Gte = Convert.ToDouble(query.MinPrice);

                if (query.MaxPrice.HasValue)
                    rangeQuery.Lte = Convert.ToDouble(query.MaxPrice);

                filterQueries.Add(Query.Range(rangeQuery));
            }

            // Category filter
            if (query.CategoryId.HasValue)
            {
                filterQueries.Add(new TermQuery("categoryId")
                {
                    Value = query.CategoryId.Value
                });
            }
            else if (query.ParentCategoryId.HasValue)
            {
                filterQueries.Add(new TermQuery("categoryPath")
                {
                    Value = query.ParentCategoryId.Value
                });
            }
            if (query.BrandId.HasValue)
            {
                filterQueries.Add(new TermQuery("BrandId")
                {
                    Value= query.BrandId.Value
                });
            }
            if (filterQueries.Count > 0)
            {
                boolQuery.Filter = filterQueries.ToArray();
            }
            if ((boolQuery.Must == null && boolQuery.Filter == null) || (boolQuery.Must.Count == 0 && boolQuery.Filter.Count == 0))
            {
                searchRequest.Query = new MatchAllQuery();
            }
            else
            {
                searchRequest.Query = Query.Bool(boolQuery);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                searchRequest.Sort = new List<SortOptions>();

                switch (query.SortBy.ToLower())
                {
                    case "price-asc":
                        searchRequest.Sort.Add(SortOptions.Field("price",new FieldSort {Order=SortOrder.Asc }));
                        break;
                    case "price-desc":
                        searchRequest.Sort.Add(SortOptions.Field("price", new FieldSort { Order = SortOrder.Desc }));
                        break;
                    case "name-asc":
                        searchRequest.Sort.Add(SortOptions.Field("name",new FieldSort {Order=SortOrder.Asc }));
                        break;
                    case "name-desc":
                        searchRequest.Sort.Add(SortOptions.Field("name", new FieldSort { Order = SortOrder.Desc }));
                        break;
                }
            }

            var response = await _elasticClient.SearchAsync<ProductDocument>(searchRequest);

            return response.IsValidResponse ? response.Documents.Select(d => new Product
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                CategoryId = d.CategoryId,
                CreatedDate = d.CreatedDate
            }).ToList() : new List<Product>();
        }



        public async Task IndexProductAsync(ProductDocument product)
        {
            await _elasticClient.IndexAsync(product, i => i.Index("products"));
        }
        public async Task DeleteProductAsync(int id)
        {
            await _elasticClient.DeleteAsync<Product>(id, idx => idx.Index("products"));
        }
    }

}
