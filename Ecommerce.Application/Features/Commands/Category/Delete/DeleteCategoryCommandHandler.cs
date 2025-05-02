using Ecommerce.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Commands.Category.Delete
{
    public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task HandleAsync(DeleteCategoryCommand command)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(command.Id);
                if (category == null)
                {
                    throw new Exception($"Category with ID {command.Id} not found");
                }

                await _categoryRepository.DeleteCategoryAsync(command.Id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting category: {ex.Message}", ex);
            }
        }
    }
}