using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Product)
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByProductIdAsync(int productId)
        {
            return await _context.Orders
                .Include(o => o.Product)
                .Where(o => o.ProductId == productId)
                .ToListAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }

}

