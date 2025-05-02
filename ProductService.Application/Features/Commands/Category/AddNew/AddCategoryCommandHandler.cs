using ProductService.Application.Interfaces;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Commands.Category.AddNew
{
    public class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public AddCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task HandleAsync(AddCategoryCommand command)
        {
            var category = new Domain.Entities.Category
            {
                Name = command.Name
            };

            await _categoryRepository.AddCategoryAsync(category);
        }
    }
}
