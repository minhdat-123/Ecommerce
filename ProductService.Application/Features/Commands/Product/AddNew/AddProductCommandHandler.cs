using ProductService.Application.Interfaces;
using ProductService.Domain.ElasticSearch.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Commands.Product.AddNew
{
    public class AddProductCommandHandler : ICommandHandler<AddProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductSearchService _productSearchService;

        public AddProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository,IBrandRepository brandRepository, IProductSearchService productSearchService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _productSearchService = productSearchService;
        }

        public async Task HandleAsync(AddProductCommand command)
        {
            var product = new Domain.Entities.Product
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                CategoryId = command.CategoryId,
                BrandId = command.BrandId,
                CreatedDate = DateTime.UtcNow
            };
            var categoryPath = await _categoryRepository.GetCategoryPathAsync(product.CategoryId);
            var brand = await _brandRepository.GetBrandByIdAsync(command.BrandId);
            var category =await _categoryRepository.GetCategoryByIdAsync(command.CategoryId);
            var document = new ProductDocument
            {
                Id = product.Id,
                Name = product.Name,
                Keyword = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName= category.Name,
                CategoryPath = categoryPath,
                BrandId = product.BrandId,
                BrandName = brand.Name,
                CreatedDate = product.CreatedDate,
                NameSuggest = new CompletionField
                {
                    input = new string[] { product.Name,brand.Name,brand.Name+" "+product.Name }
                }
            };
            await _productRepository.AddProductAsync(product);
            await _productSearchService.IndexProductAsync(document);
        }
    }

}

