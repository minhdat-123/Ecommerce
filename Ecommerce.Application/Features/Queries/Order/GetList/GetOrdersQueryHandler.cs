using Ecommerce.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Queries.Order.GetList
{
    public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, List<Domain.Entities.Order>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<Domain.Entities.Order>> HandleAsync(GetOrdersQuery query)
        {
            return await _orderRepository.GetAllOrdersAsync();
        }
    }

}
