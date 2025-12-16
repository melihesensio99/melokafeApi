using KafeApi.Application.Dtos.OrderDtos;
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
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrderWithDetailAsync()
        {
            var result = await _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.MenuItem)
                 .ThenInclude(x => x.Category).ToListAsync();
            return result;
        }

        public async Task<Order> GetOrderWithDetailAsync(int id)
        {
            var result = await _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.MenuItem)
                .ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<Order> UpdateOrderStatusToTeslimEdildi(int id)
        {
            var result = await _context.Orders.FirstOrDefaultAsync(result => result.Id == id);
            result.Status = OrderStatus.TESLIMEDILDI;
            await _context.SaveChangesAsync();
            return result;
        }
        public async Task<Order> UpdateOrderStatusToHazirlaniyor(int id)
        {
            var result = await _context.Orders.FirstOrDefaultAsync(result => result.Id == id);
            result.Status = OrderStatus.HAZIRLANIYOR;
            await _context.SaveChangesAsync();
            return result;
        }
        public async Task<Order> UpdateOrderStatusToUcretOdendi(int id)
        {
            var result = await _context.Orders.FirstOrDefaultAsync(result => result.Id == id);
            result.Status = OrderStatus.UCRETODENDI;
            await _context.SaveChangesAsync();
            return result;
        }
    }
}
