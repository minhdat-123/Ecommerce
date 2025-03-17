using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.ElasticSearch.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Commands.Product.AddNew
{
    public class AddProductCommandHandler : ICommandHandler<AddProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductSearchService _productSearchService;

        public AddProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, IProductSearchService productSearchService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productSearchService = productSearchService;
        }

        public async Task HandleAsync(AddProductCommand command)
        {
            var product = new Domain.Entities.Product
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                CategoryId=command.CategoryId
            };
            var categoryPath = await _categoryRepository.GetCategoryPathAsync(product.CategoryId);
            var document = new ProductDocument
            {
                Id = product.Id,
                Name = product.Name,
                Keyword=product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryPath = categoryPath,
                BrandId=product.BrandId,
                BrandName=product.Brand.Name,
                NameSuggest=new CompletionField() {input= new string[] { product.Name, product.Name + " " + product.Brand.Name, product.Name + " " + product.Brand.Name + " " + product.Category.Name } }
            };
            await _productRepository.AddProductAsync(product);
            await _productSearchService.IndexProductAsync(document);
        }
    }

}
