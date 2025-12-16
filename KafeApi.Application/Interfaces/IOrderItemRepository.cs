using KafeApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<OrderItem> GetOrderItemByDetailAsync(int id);
        Task<List<OrderItem>> GetAllOrderItemByDetailAsync();


    }
}
