using ProductService.Domain.Entities;
using System.Threading.Tasks;

namespace ProductService.Application.Interfaces
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetBrandsByCategoryIdAsync(int categoryId);
        Task<Brand> GetBrandByIdAsync(int id);
    }
}
