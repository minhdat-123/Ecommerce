using Ecommerce.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface ISearchConfigRepository
    {
        Task<List<SearchConfig>> GetAllConfigsAsync();
        Task<List<SearchConfig>> GetConfigsByTypeAsync(SearchConfigType type);
        Task<SearchConfig> GetConfigByIdAsync(int id);
        Task AddConfigAsync(SearchConfig config);
        Task UpdateConfigAsync(SearchConfig config);
        Task DeleteConfigAsync(int id);
    }
}