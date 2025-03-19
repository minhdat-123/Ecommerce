using Ecommerce.Application.Features.Commands.Product.AddNew;
using Ecommerce.Application.Features.Commands.Product.Delete;
using Ecommerce.Application.Features.Commands.Product.Index;
using Ecommerce.Application.Features.Queries.Product.GetList;
using Ecommerce.Application.Features.Queries.Product.Search;
using Ecommerce.Application.Features.Queries.Product.Suggestion;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ICommandHandler<AddProductCommand> _addProductHandler;
        private readonly ICommandHandler<DeleteProductCommand> _deleteProductHandler;
        private readonly ICommandHandler<IndexProductsCommand> _indexProductsHandler;
        private readonly IQueryHandler<GetProductsQuery, List<Domain.Entities.Product>> _getProductsHandler;
        private readonly IQueryHandler<SearchProductsQuery, List<Domain.Entities.Product>> _searchProductsHandler;
        private readonly IQueryHandler<SuggestionSearchQuery, List<string>> _suggestionSearchHandler;

        public ProductController(
            ICommandHandler<AddProductCommand> addProductHandler,
            ICommandHandler<DeleteProductCommand> deleteProductHandler,
            ICommandHandler<IndexProductsCommand> indexProductsHandler,
            IQueryHandler<GetProductsQuery, List<Product>> getProductsHandler,
            IQueryHandler<SearchProductsQuery, List<Product>> searchProductsHandler,
            IQueryHandler<SuggestionSearchQuery, List<string>> suggestionSearchHandler)
        {
            _addProductHandler = addProductHandler;
            _deleteProductHandler = deleteProductHandler;
            _indexProductsHandler = indexProductsHandler;
            _getProductsHandler = getProductsHandler;
            _searchProductsHandler = searchProductsHandler;
            _suggestionSearchHandler = suggestionSearchHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] AddProductCommand command)
        {
            await _addProductHandler.HandleAsync(command);
            return CreatedAtAction(nameof(GetProducts), null);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var query = new GetProductsQuery();
            var products = await _getProductsHandler.HandleAsync(query);
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(
              [FromQuery] string query = "",
              [FromQuery] decimal? minPrice = null,
              [FromQuery] decimal? maxPrice = null,
              [FromQuery] int? categoryId = null,
              [FromQuery] int? parentCategoryId=null,
              [FromQuery] int? brandId=null,
              [FromQuery] string sortBy = "",
              [FromQuery] int page = 1,
              [FromQuery] int pageSize = 10)
        {
            var searchQuery = new SearchProductsQuery
            {
                Query = query,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                CategoryId = categoryId,
                ParentCategoryId=parentCategoryId,
                BrandId= brandId,
                SortBy = sortBy,
                Page = page,
                PageSize = pageSize
            };
            var products = await _searchProductsHandler.HandleAsync(searchQuery);
            return Ok(products);
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSuggestions([FromQuery] string query)
        {
            var suggestionQuery = new SuggestionSearchQuery
            {
                Query = query
            };
            var suggestions = await _suggestionSearchHandler.HandleAsync(suggestionQuery);
            return Ok(suggestions);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var command = new DeleteProductCommand { Id = id };
                await _deleteProductHandler.HandleAsync(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("index")]
        public async Task<IActionResult> IndexProducts()
        {
            try
            {
                var command = new IndexProductsCommand();
                await _indexProductsHandler.HandleAsync(command);
                return Ok(new { message = "Products indexed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
