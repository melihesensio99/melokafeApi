using KafeApi.Application.Dtos.OrderItemDtos;
using KafeApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
       
        public int TableId { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; }

    }
}
