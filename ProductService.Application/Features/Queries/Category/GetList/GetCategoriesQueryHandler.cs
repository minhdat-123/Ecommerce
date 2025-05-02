using System.Collections.Generic;
using System.Threading.Tasks;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Application.Features.Queries.Category.GetList
{
    public class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, List<Domain.Entities.Category>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Domain.Entities.Category>> HandleAsync(GetCategoriesQuery query)
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }
    }
}
