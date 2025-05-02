using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.Queries.Category.GetSub
{
    public class GetSubcategoriesQuery
    {
        public int CategoryId { get; set; }
    }

    public class GetSubcategoriesQueryHandler : IQueryHandler<GetSubcategoriesQuery, List<Domain.Entities.Category>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetSubcategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Domain.Entities.Category>> HandleAsync(GetSubcategoriesQuery query)
        {
            return await _categoryRepository.GetSubcategoriesAsync(query.CategoryId);
        }
    }
}