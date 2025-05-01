using ProductService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Commands.Order.AddNew
{
    public class AddOrderCommandHandler : ICommandHandler<AddOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;

        public AddOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(AddOrderCommand command)
        {
            var order = new Domain.Entities.Order
            {
                ProductId = command.ProductId,
                Quantity = command.Quantity,
                OrderDate = DateTime.UtcNow
            };
            await _orderRepository.AddOrderAsync(order);
        }
    }

}

