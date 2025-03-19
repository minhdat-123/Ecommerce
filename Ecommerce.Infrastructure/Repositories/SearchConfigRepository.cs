using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class SearchConfigRepository : ISearchConfigRepository
    {
        private readonly AppDbContext _context;

        public SearchConfigRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SearchConfig>> GetAllConfigsAsync()
        {
            return await _context.SearchConfigs.ToListAsync();
        }

        public async Task<List<SearchConfig>> GetConfigsByTypeAsync(SearchConfigType type)
        {
            return await _context.SearchConfigs
                .Where(sc => sc.Type == type && sc.IsActive)
                .ToListAsync();
        }

        public async Task<SearchConfig> GetConfigByIdAsync(int id)
        {
            return await _context.SearchConfigs.FindAsync(id);
        }

        public async Task AddConfigAsync(SearchConfig config)
        {
            config.CreatedDate = DateTime.UtcNow;
            _context.SearchConfigs.Add(config);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateConfigAsync(SearchConfig config)
        {
            config.UpdatedDate = DateTime.UtcNow;
            _context.SearchConfigs.Update(config);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteConfigAsync(int id)
        {
            var config = await _context.SearchConfigs.FindAsync(id);
            if (config != null)
            {
                _context.SearchConfigs.Remove(config);
                await _context.SaveChangesAsync();
            }
        }
    }
}