using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.ElasticSearch.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Commands.Product.Index
{
    public class IndexProductsCommandHandler : ICommandHandler<IndexProductsCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductSearchService _productSearchService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;

        public IndexProductsCommandHandler(
            IProductRepository productRepository,
            IProductSearchService productSearchService,
            ICategoryRepository categoryRepository,
            IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _productSearchService = productSearchService;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
        }

        public async Task HandleAsync(IndexProductsCommand command)
        {
            // Get all products from the database
            var products = await _productRepository.GetAllProductsAsync();
            
            // Index each product in Elasticsearch
            foreach (var product in products)
            {
                // Get category information
                var category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
                var categoryPath = new List<int>();
                
                // Add parent categories to the path if available
                if (category != null && category.ParentCategoryId.HasValue)
                {
                    categoryPath.Add(category.ParentCategoryId.Value);
                }
                
                // Get brand information
                var brand = await _brandRepository.GetBrandByIdAsync(product.BrandId);
                
                // Create the product document for indexing
                var productDocument = new ProductDocument
                {
                    Id = product.Id,
                    Name = product.Name,
                    Keyword = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    CategoryName = category?.Name,
                    CategoryPath = categoryPath,
                    BrandId = product.BrandId,
                    BrandName = brand?.Name,
                    CreatedDate = product.CreatedDate,
                    NameSuggest = new CompletionField
                    {
                        input = new[] { product.Name,brand.Name+" "+product.Name,brand.Name+" "+product.Name+" "+category.Name }
                    }
                };
                
                // Index the product in Elasticsearch
                await _productSearchService.IndexProductAsync(productDocument);
            }
        }
    }
}