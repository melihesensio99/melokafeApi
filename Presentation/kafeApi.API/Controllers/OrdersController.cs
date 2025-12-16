using KafeApi.Application.Dtos.OrderDtos;
using KafeApi.Application.Dtos.OrderItemDtos;
using KafeApi.Application.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace kafeApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderItem()
        {
            var result = await _orderService.GetAllOrder();
            return CreateResponse(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItemById(int id)
        {
            var result = await _orderService.GetOrderById(id);
            return CreateResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderItem(CreateOrderDto createOrderDto)
        {
            var result = await _orderService.AddOrder(createOrderDto);
            return CreateResponse(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateOrderItem(UpdateOrderDto updateOrderDto)
        {
            var result = await _orderService.UpdateOrder(updateOrderDto);
            return CreateResponse(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var result = await _orderService.DeleteOrder(id);
            return CreateResponse(result);
        }
        [HttpPut("updateorderstatustoteslimedildi")]
        public async Task<IActionResult> UpdateOrderStatusToTeslimEdildi(int id)
        {
            var result = await _orderService.UpdateOrderStatusToTeslimEdildiById(id);
            return CreateResponse(result);
        }
        [HttpPut("updateorderstatustoyolda")]
        public async Task<IActionResult> UpdateOrderStatusToYolda(int id)
        {
            var result = await _orderService.UpdateOrderStatusToHazirlaniyor(id);
            return CreateResponse(result);
        }
        [HttpPut("updateorderstatustoucretodendi")]
        public async Task<IActionResult> UpdateOrderStatusToUcretOdendi(int id)
        {
            var result = await _orderService.UpdateOrderStatusToUcretOdendi(id);
            return CreateResponse(result);
        }

    }
}
