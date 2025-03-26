using Ecommerce.Application.Features.Queries.Product.Search;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.ElasticSearch.Documents;
using Ecommerce.Domain.Entities;
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

namespace Ecommerce.Infrastructure.Services
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
            // Create a SearchRequest object instead of descriptor
            var searchRequest = new SearchRequest<ProductDocument>("products")
            {
                From = (query.Page - 1) * query.PageSize,
                Size = query.PageSize,
                Source = new SourceConfig(new SourceFilter
                {
                    Includes = new[] { "id", "name", "description", "price", "categoryName", "brandName", "createdDate" }
                }),
                TrackTotalHits = new TrackHits(true)
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
                            Fields = new[] { "name", "description", "categoryName", "brandName" },
                            Query = query.Query,
                            //MinimumShouldMatch=MinimumShouldMatch.Percentage(100),
                            Fuzziness = new Fuzziness("AUTO")
                        }
};
                searchRequest.Highlight = new Highlight
                {
                    PreTags = new[] { "<em>" },
                    PostTags = new[] { "</em>" },
                    Fields = new Dictionary<Field, HighlightField>
                    {
                        { Infer.Field<ProductDocument>(p => p.Name), new HighlightField() },
                        { Infer.Field<ProductDocument>(p => p.Description), new HighlightField {
                            FragmentSize = 150,
                            NumberOfFragments = 1
                        }}
                    }
                };
            }

            // Add filter clauses
            var filterQueries = new List<Query>();

            // Price range filter
            if (query.MinPrice.HasValue || query.MaxPrice.HasValue)
            {
                var rangeQuery = new NumberRangeQuery(Infer.Field<ProductDocument>(p => p.Price));
                if (query.MinPrice.HasValue)
                    rangeQuery.Gte = Convert.ToDouble(query.MinPrice);

                if (query.MaxPrice.HasValue)
                    rangeQuery.Lte = Convert.ToDouble(query.MaxPrice);

                filterQueries.Add(Query.Range(rangeQuery));
            }

            // Category filter
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
            if (filterQueries.Count > 0)
            {
                boolQuery.Filter = filterQueries.ToArray();
            }
            if (boolQuery.Must == null && boolQuery.Filter == null)
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
                        searchRequest.Sort.Add(SortOptions.Field(
                            Infer.Field<ProductDocument>(p => p.Price),
                            new FieldSort { Order = SortOrder.Asc }));
                        break;
                    case "price-desc":
                        searchRequest.Sort.Add(SortOptions.Field(
                            Infer.Field<ProductDocument>(p => p.Price),
                            new FieldSort { Order = SortOrder.Desc }));
                        break;
                    case "name-asc":
                        searchRequest.Sort.Add(SortOptions.Field(
                            Infer.Field<ProductDocument>(p => p.Keyword),
                            new FieldSort { Order = SortOrder.Asc }));
                        break;
                    case "name-desc":
                        searchRequest.Sort.Add(SortOptions.Field(
                            Infer.Field<ProductDocument>(p => p.Keyword),
                            new FieldSort { Order = SortOrder.Desc }));
                        break;
                    case "newest":
                        searchRequest.Sort.Add(SortOptions.Field(
                            Infer.Field<ProductDocument>(p => p.CreatedDate),
                            new FieldSort { Order = SortOrder.Asc }));
                        break;
                }
                searchRequest.Sort.Add(SortOptions.Field(Infer.Field<ProductDocument>(p => p.Id), new FieldSort { Order = SortOrder.Asc }));
            }

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
