using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Application.Features.Queries.Category.GetTopLevel
{
    public class GetTopLevelCategoriesQuery { }

    public class GetTopLevelCategoriesQueryHandler : IQueryHandler<GetTopLevelCategoriesQuery, List<Domain.Entities.Category>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetTopLevelCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Domain.Entities.Category>> HandleAsync(GetTopLevelCategoriesQuery query)
        {
            return await _categoryRepository.GetTopLevelCategoriesAsync();
        }
    }
}
