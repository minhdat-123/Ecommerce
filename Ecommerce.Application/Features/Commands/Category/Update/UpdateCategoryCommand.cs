using System;

namespace Ecommerce.Application.Features.Commands.Category.Update
{
    public class UpdateCategoryCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}