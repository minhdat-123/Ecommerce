using Ecommerce.Application.Features.Queries.Brands;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IQueryHandler<GetBrandsByCategoryQuery, List<Brand>> _getBrandsByCategoryHandler;

        public BrandController(IQueryHandler<GetBrandsByCategoryQuery, List<Brand>> getBrandsByCategoryHandler)
        {
            _getBrandsByCategoryHandler = getBrandsByCategoryHandler;
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<List<Brand>>> GetBrandsByCategory(int categoryId)
        {
            var query = new GetBrandsByCategoryQuery { CategoryId = categoryId };
            var brands = await _getBrandsByCategoryHandler.HandleAsync(query);
            return Ok(brands);
        }
    }
}