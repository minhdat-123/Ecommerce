using Ecommerce.Domain.Entities;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetBrandsByCategoryIdAsync(int categoryId);
        Task<Brand> GetBrandByIdAsync(int id);
    }
}