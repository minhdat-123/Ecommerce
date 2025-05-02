using Ecommerce.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Queries.Product.GetList
{
    public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, List<Domain.Entities.Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Domain.Entities.Product>> HandleAsync(GetProductsQuery query)
        {
            return await _productRepository.GetAllProductsAsync();
        }
    }

}
