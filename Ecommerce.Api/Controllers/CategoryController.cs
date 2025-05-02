using Ecommerce.Application.Features.Commands.Category.AddNew;
using Ecommerce.Application.Features.Commands.Category.Update;
using Ecommerce.Application.Features.Commands.Category.Delete;
using Ecommerce.Application.Features.Queries.Category.GetList;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Domain.Entities;
using Ecommerce.Application.Features.Queries.Category.GetSub;
using Ecommerce.Application.Features.Queries.Category.GetTopLevel;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICommandHandler<AddCategoryCommand> _addCategoryHandler;
        private readonly ICommandHandler<UpdateCategoryCommand> _updateCategoryHandler;
        private readonly ICommandHandler<DeleteCategoryCommand> _deleteCategoryHandler;
        private readonly IQueryHandler<GetCategoriesQuery, List<Category>> _getCategoriesHandler;
        private readonly IQueryHandler<GetTopLevelCategoriesQuery, List<Category>> _getTopLevelCategoriesHandler;
        private readonly IQueryHandler<GetSubcategoriesQuery, List<Category>> _getSubcategoriesHandler;

        public CategoryController(
            ICommandHandler<AddCategoryCommand> addCategoryHandler,
            ICommandHandler<UpdateCategoryCommand> updateCategoryHandler,
            ICommandHandler<DeleteCategoryCommand> deleteCategoryHandler,
            IQueryHandler<GetCategoriesQuery, List<Category>> getCategoriesHandler,
            IQueryHandler<GetTopLevelCategoriesQuery, List<Category>> getTopLevelCategoriesHandler,
            IQueryHandler<GetSubcategoriesQuery, List<Category>> getSubcategoriesHandler)
        {
            _addCategoryHandler = addCategoryHandler;
            _updateCategoryHandler = updateCategoryHandler;
            _deleteCategoryHandler = deleteCategoryHandler;
            _getCategoriesHandler = getCategoriesHandler;
            _getTopLevelCategoriesHandler = getTopLevelCategoriesHandler;
            _getSubcategoriesHandler = getSubcategoriesHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryCommand command)
        {
            try
            {
                await _addCategoryHandler.HandleAsync(command);
                return CreatedAtAction(nameof(AddCategory), null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryCommand command)
        {
            try
            {
                if (id != command.Id)
                {
                    return BadRequest("ID mismatch");
                }
                await _updateCategoryHandler.HandleAsync(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var command = new DeleteCategoryCommand { Id = id };
                await _deleteCategoryHandler.HandleAsync(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var query = new GetCategoriesQuery();
                var categories = await _getCategoriesHandler.HandleAsync(query);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("top-level")]
        public async Task<IActionResult> GetTopLevelCategories()
        {
            try
            {
                var query = new GetTopLevelCategoriesQuery();
                var categories = await _getTopLevelCategoriesHandler.HandleAsync(query);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("subcategories/{categoryId}")]
        public async Task<IActionResult> GetSubcategories(int categoryId)
        {
            try
            {
                var query = new GetSubcategoriesQuery { CategoryId = categoryId };
                var subcategories = await _getSubcategoriesHandler.HandleAsync(query);
                return Ok(subcategories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}