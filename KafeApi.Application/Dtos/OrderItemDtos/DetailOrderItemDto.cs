using KafeApi.Application.Dtos.MenuItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Dtos.OrderItemDtos
{
    public class DetailOrderItemDto
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public int OrderId { get; set; }
        public DetailMenuItemDto MenuItem { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
