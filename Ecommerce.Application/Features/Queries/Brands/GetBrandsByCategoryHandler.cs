using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.Queries.Brands
{
    public class GetBrandsByCategoryQuery
    {
        public int CategoryId { get; set; }
    }

    public class GetBrandsByCategoryHandler : IQueryHandler<GetBrandsByCategoryQuery, List<Domain.Entities.Brand>>
    {
        private readonly IBrandRepository _brandRepository;

        public GetBrandsByCategoryHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<List<Brand>> HandleAsync(GetBrandsByCategoryQuery query)
        {
            return await _brandRepository.GetBrandsByCategoryIdAsync(query.CategoryId);
        }
    }
}