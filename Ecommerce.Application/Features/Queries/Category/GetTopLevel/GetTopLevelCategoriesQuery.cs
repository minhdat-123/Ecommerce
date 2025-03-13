using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.Queries.Category.GetTopLevel
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