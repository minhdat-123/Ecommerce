using Ecommerce.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Commands.Product.Delete
{
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductSearchService _productSearchService;

        public DeleteProductCommandHandler(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IProductSearchService productSearchService)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _productSearchService = productSearchService;
        }

        public async Task HandleAsync(DeleteProductCommand command)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(command.Id);
                if (product == null)
                {
                    throw new Exception($"Product with ID {command.Id} not found");
                }

                // Delete associated orders first
                var orders = await _orderRepository.GetOrdersByProductIdAsync(command.Id);
                foreach (var order in orders)
                {
                    await _orderRepository.DeleteOrderAsync(order.Id);
                }

                // Remove product from Elasticsearch
                await _productSearchService.DeleteProductAsync(command.Id);

                // Finally, delete the product from the database
                await _productRepository.DeleteProductAsync(command.Id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting product: {ex.Message}", ex);
            }
        }
    }
}