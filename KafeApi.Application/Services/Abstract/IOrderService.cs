using KafeApi.Application.Dtos.OrderDtos;
using KafeApi.Application.Dtos.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Services.Abstract
{
    public interface IOrderService
    {
        public Task<ResponseDto<List<ResultOrderDto>>> GetAllOrder();
        public Task<ResponseDto<DetailOrderDto>> GetOrderById(int id);
        Task<ResponseDto<object>> AddOrder(CreateOrderDto createOrderDto);
        Task<ResponseDto<object>> UpdateOrder(UpdateOrderDto updateOrderDto);
        Task<ResponseDto<object>> DeleteOrder(int id);
        Task<ResponseDto<object>> UpdateOrderStatusToTeslimEdildiById(int id);
        Task<ResponseDto<object>> UpdateOrderStatusToHazirlaniyor(int id);
        Task<ResponseDto<object>> UpdateOrderStatusToUcretOdendi(int id);

    }
}
