using ProductService.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Commands.Category.Update
{
    public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task HandleAsync(UpdateCategoryCommand command)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(command.Id);
                if (category == null)
                {
                    throw new Exception($"Category with ID {command.Id} not found");
                }

                category.Name = command.Name;
                await _categoryRepository.UpdateCategoryAsync(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating category: {ex.Message}", ex);
            }
        }
    }
}
