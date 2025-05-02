using Ecommerce.Application.Features.Commands.Order.AddNew;
using Ecommerce.Application.Features.Queries.Order.GetList;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly ICommandHandler<AddOrderCommand> _addOrderHandler;
        private readonly IQueryHandler<GetOrdersQuery, List<Domain.Entities.Order>> _getOrdersHandler;

        public OrderController(
            ICommandHandler<AddOrderCommand> addOrderHandler,
            IQueryHandler<GetOrdersQuery, List<Order>> getOrdersHandler)
        {
            _addOrderHandler = addOrderHandler;
            _getOrdersHandler = getOrdersHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderCommand command)
        {
            await _addOrderHandler.HandleAsync(command);
            return CreatedAtAction(nameof(GetOrders), null);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var query = new GetOrdersQuery();
            var orders = await _getOrdersHandler.HandleAsync(query);
            return Ok(orders);
        }
    }

}
