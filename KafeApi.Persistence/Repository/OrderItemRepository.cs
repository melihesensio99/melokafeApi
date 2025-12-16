using KafeApi.Application.Interfaces;
using KafeApi.Domain.Entities;
using KafeApi.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Persistence.Repository
{

    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _context;
        public OrderItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderItem>> GetAllOrderItemByDetailAsync()
        {
            var result = await _context.OrderItems.Include(x => x.MenuItem).ThenInclude(x => x.Category).ToListAsync();
            return result;
      
        }

        public async Task<OrderItem> GetOrderItemByDetailAsync(int id)
        {
            var result = await _context.OrderItems.Include(x => x.MenuItem).ThenInclude(x => x.Category).FirstOrDefaultAsync(x=>x.Id == id);

            return result;
        }
    }
}
