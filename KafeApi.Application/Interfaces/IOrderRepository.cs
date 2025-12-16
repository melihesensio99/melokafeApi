using KafeApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrderWithDetailAsync();
        Task<Order> GetOrderWithDetailAsync(int id);
        Task<Order> UpdateOrderStatusToTeslimEdildi(int id);
        Task<Order> UpdateOrderStatusToHazirlaniyor(int id);
        Task<Order> UpdateOrderStatusToUcretOdendi(int id);
    }
}
