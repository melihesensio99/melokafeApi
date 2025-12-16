using KafeApi.Application.Dtos.OrderDtos;
using KafeApi.Application.Dtos.OrderItemDtos;
using KafeApi.Application.Dtos.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Services.Abstract
{
    public interface IOrderItemService
    {
        public Task<ResponseDto<List<ResultOrderItemDto>>> GetAllOrderItem();
        public Task<ResponseDto<DetailOrderItemDto>> GetOrderItemById(int id);
        Task<ResponseDto<object>> AddOrderItem(CreateOrderItemDto createOrderItemDto);
        Task<ResponseDto<object>> UpdateOrderItem(UpdateOrderItemDto updateOrderItemDto);
        Task<ResponseDto<object>> DeleteOrderItem(int id);

    }
}
