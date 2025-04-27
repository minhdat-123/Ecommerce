using ProductService.Application.Features.Queries.Product.Search;
using ProductService.Application.Interfaces;
using ProductService.Domain.ElasticSearch.Documents;
using ProductService.Domain.Entities;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.Mapping;
using Elastic.Clients.Elasticsearch.Nodes;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Services
{
    public class ProductSearchService : IProductSearchService
    {
        private readonly ElasticsearchClient _elasticClient;
        private readonly ISearchConfigRepository _searchConfigRepository;
        private readonly ILogger<ProductSearchService> _logger;

        public ProductSearchService(ElasticsearchClient elasticClient, ISearchConfigRepository searchConfigRepository, ILogger<ProductSearchService> logger)
        {
            _elasticClient = elasticClient;
            _searchConfigRepository = searchConfigRepository;
            _logger = logger;
            //Task.Run(async () => await CreateProductsIndexAsync()).Wait();
        }

        public async Task<List<ProductDocument>> SearchProductsAsync(string query)
        {
            var result = await SearchProductsAsync(new SearchProductsQuery { Query = query });
            return result.Products;
        }

        public async Task<(List<ProductDocument> Products, long TotalCount)> SearchProductsAsync(SearchProductsQuery query)
        {
            // Validate input
            query.Page = Math.Max(1, query.Page);
            query.PageSize = Math.Clamp(query.PageSize, 1, 100);

            var searchRequest = CreateSearchRequest(query);

            try
            {
                var response = await _elasticClient.SearchAsync<ProductDocument>(searchRequest);

                if (!response.IsValidResponse)
                {
                    _logger.LogError("Elasticsearch search error: {ErrorDetails}", response.DebugInformation);
                    return (new List<ProductDocument>(), 0);
                }

                return (response.Documents.ToList(), response.Total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during Elasticsearch search");
                return (new List<ProductDocument>(), 0);
            }
        }

        private SearchRequest<ProductDocument> CreateSearchRequest(SearchProductsQuery query)
        {
            var searchRequest = new SearchRequest<ProductDocument>("products")
            {
                From = (query.Page - 1) * query.PageSize,
                Size = query.PageSize,
                Source = new SourceConfig(new SourceFilter
                {
                    Includes = new[] { "id", "name", "description", "price", "categoryName", "brandName", "createdDate" }
                }),
                TrackTotalHits = new TrackHits(true),
                Query = BuildBoolQuery(query),
                Sort = BuildSortOptions(query)
            };

            return searchRequest;
        }

        private Query BuildBoolQuery(SearchProductsQuery query)
        {
            var boolQuery = new BoolQuery
            {
                Must = BuildMustQuery(query),
                Filter = BuildFilterQuery(query)
            };

            return boolQuery.Must == null && boolQuery.Filter == null ? new MatchAllQuery() : Query.Bool(boolQuery);
        }

        private Query[]? BuildMustQuery(SearchProductsQuery query)
        {
            if (string.IsNullOrEmpty(query.Query))
                return null;

            return new Query[]
            {
        new MultiMatchQuery()
        {
            Fields = new[] { "name", "description", "categoryName", "brandName" },
            Query = query.Query,
            Fuzziness = new Fuzziness("AUTO")
        }
            };
        }

        private Query[]? BuildFilterQuery(SearchProductsQuery query)
        {
            var filterQueries = new List<Query>();

            // Price range filter
            if (query.MinPrice.HasValue || query.MaxPrice.HasValue)
            {
                var rangeQuery = new NumberRangeQuery(Infer.Field<ProductDocument>(p => p.Price))
                {
                    Gte = query.MinPrice.HasValue ? Convert.ToDouble(query.MinPrice) : null,
                    Lte = query.MaxPrice.HasValue ? Convert.ToDouble(query.MaxPrice) : null
                };
                filterQueries.Add(Query.Range(rangeQuery));
            }

            // Category or Parent Category filter
            if (query.CategoryId.HasValue)
            {
                filterQueries.Add(new TermQuery(Infer.Field<ProductDocument>(p => p.CategoryId))
                {
                    Value = query.CategoryId.Value
                });
            }
            else if (query.ParentCategoryId.HasValue)
            {
                filterQueries.Add(new TermQuery(Infer.Field<ProductDocument>(p => p.CategoryPath))
                {
                    Value = query.ParentCategoryId.Value
                });
            }

            // Brand filter
            if (query.BrandId.HasValue)
            {
                filterQueries.Add(new TermQuery(Infer.Field<ProductDocument>(p => p.BrandId))
                {
                    Value = query.BrandId.Value
                });
            }

            return filterQueries.Count > 0 ? filterQueries.ToArray() : null;
        }


        private List<SortOptions> BuildSortOptions(SearchProductsQuery query)
        {
            var sortOptions = new List<SortOptions>();

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                switch (query.SortBy.ToLower())
                {
                    case "price-asc":
                        sortOptions.Add(SortOptions.Field(Infer.Field<ProductDocument>(p => p.Price), new FieldSort { Order = SortOrder.Asc }));
                        break;
                    case "price-desc":
                        sortOptions.Add(SortOptions.Field(Infer.Field<ProductDocument>(p => p.Price), new FieldSort { Order = SortOrder.Desc }));
                        break;
                    case "name-asc":
                        sortOptions.Add(SortOptions.Field(Infer.Field<ProductDocument>(p => p.Keyword), new FieldSort { Order = SortOrder.Asc }));
                        break;
                    case "name-desc":
                        sortOptions.Add(SortOptions.Field(Infer.Field<ProductDocument>(p => p.Keyword), new FieldSort { Order = SortOrder.Desc }));
                        break;
                    case "newest":
                        sortOptions.Add(SortOptions.Field(Infer.Field<ProductDocument>(p => p.CreatedDate), new FieldSort { Order = SortOrder.Asc }));
                        break;
                }
            }

            sortOptions.Add(SortOptions.Field(Infer.Field<ProductDocument>(p => p.Id), new FieldSort { Order = SortOrder.Asc }));

            return sortOptions;
        }

        public async Task<List<string>> SuggestionSearchAsync(string query)
        {
            var suggestResponse = await GetCompletionSuggestionsAsync("products", "nameSuggest", query, 10);
            return suggestResponse.SelectMany(o => o.Options).Select(o => o.Text).ToList();
        }


        public async Task IndexProductAsync(ProductDocument product)
        {
            // Ensure the index exists before indexing
            await EnsureIndexExistsAsync();

            // Index the product
            await _elasticClient.IndexAsync(product, i => i.Index("products"));
        }
        public async Task DeleteProductAsync(int id)
        {
            await _elasticClient.DeleteAsync<Product>(id, idx => idx.Index("products"));
        }
        private async Task CreateProductsIndexAsync()
        {
            var indexName = "products";

            var existsResponse = await _elasticClient.Indices.ExistsAsync(indexName);
            if (existsResponse.Exists)
            {
                return;
            }

            // Get active stemming configuration
            var stemmingConfigs = await _searchConfigRepository.GetConfigsByTypeAsync(SearchConfigType.Stemming);
            var activeStemmer = stemmingConfigs.FirstOrDefault(sc => sc.IsActive)?.Value ?? "english";

            // Get active synonym configuration
            var synonymConfigs = await _searchConfigRepository.GetConfigsByTypeAsync(SearchConfigType.Synonym);
            var activeSynonyms = synonymConfigs.FirstOrDefault(sc => sc.IsActive)?.Value ?? string.Empty;
            var synonymsList = ParseSynonyms(activeSynonyms);

            var createIndexResponse = await _elasticClient.Indices.CreateAsync<ProductDocument>(
                indexName,
                c => c
                    .Settings(s => s
                        .Analysis(a => a
                            .Analyzers(an => an
                                .Custom("product_analyzer", ca => ca
                                    .Tokenizer("standard")
                                    .Filter(new string[] { "lowercase", "product_stemmer", "product_synonyms" })
                                )
                            )
                            .TokenFilters(tf => tf
                                .Stemmer("product_stemmer", st => st
                                    .Language(activeStemmer)
                                )
                                .SynonymGraph("product_synonyms", sy => sy
                                    .Synonyms(synonymsList.ToArray())
                                )
                            )
                        )
                    )
                    .Mappings(m => m.Properties(per => per
                        .IntegerNumber(i => i.Id)
                        .Text(i => i.Name, t => t.Analyzer("product_analyzer"))
                        .Keyword(k => k.Keyword)
                        .Text(i => i.Description, t => t.Analyzer("product_analyzer"))
                        .DoubleNumber(i => i.Price)
                        .IntegerNumber(i => i.CategoryId)
                        .Text(i => i.CategoryName)
                        .IntegerNumber(i => i.CategoryPath)
                        .IntegerNumber(i => i.BrandId)
                        .Text(i => i.BrandName)
                        .Date(i => i.CreatedDate)
                        .Completion(i => i.NameSuggest)
                    ))
            );

            if (!createIndexResponse.IsValidResponse)
            {
                throw new Exception($"Failed to create index: {createIndexResponse.DebugInformation}");
            }
        }
        private async Task<IReadOnlyCollection<CompletionSuggest<ProductDocument>>> GetCompletionSuggestionsAsync(string indexName, string fieldName, string prefix, int size = 5)
        {
            var response = await _elasticClient.SearchAsync<ProductDocument>(s => s
            .Index(indexName)
            .Size(0)
            .Suggest(sg => sg
            .Suggesters(st => st.Add("completion-suggestions", fc => fc.Completion(new CompletionSuggester
            {
                Field = fieldName,
                Size = size,
                SkipDuplicates = true
            }).Prefix(prefix)))
            )
            );

            if (!response.IsValidResponse)
            {
                throw new Exception($"Error getting completion suggestions: {response.ElasticsearchServerError?.Error}");
            }
            return response.Suggest.GetCompletion("completion-suggestions");
        }
        private async Task EnsureIndexExistsAsync()
        {
            var indexName = "products";

            var existsResponse = await _elasticClient.Indices.ExistsAsync(indexName);
            if (!existsResponse.Exists)
            {
                await CreateProductsIndexAsync();
            }
        }
        private List<string> ParseSynonyms(string synonymsText)
        {
            if (string.IsNullOrWhiteSpace(synonymsText))
                return new List<string>();

            return synonymsText
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToList();
        }
    }
}

