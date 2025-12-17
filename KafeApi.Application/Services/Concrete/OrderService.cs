using AutoMapper;
using KafeApi.Application.Dtos.OrderDtos;
using KafeApi.Application.Dtos.ResponseDtos;
using KafeApi.Application.Interfaces;
using KafeApi.Application.Services.Abstract;
using KafeApi.Application.Validators.Order;
using KafeApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _genericRepository;
        private readonly CreateOrderValidator _validationRules;
        private readonly UpdateOrderValidator _validationRules1;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<MenuItem> _menuItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IGenericRepository<Table> _tableRepository;
        public OrderService(IGenericRepository<Order> genericRepository, CreateOrderValidator validationRules, IMapper mapper, UpdateOrderValidator validationRules1, IOrderItemRepository orderItemRepository, IGenericRepository<MenuItem> menuItemRepository, IOrderRepository orderRepository, IGenericRepository<Table> tableRepository)
        {
            _genericRepository = genericRepository;
            _validationRules = validationRules;
            _validationRules1 = validationRules1;
            _mapper = mapper;
            _menuItemRepository = menuItemRepository;
            _orderRepository = orderRepository;
            _tableRepository = tableRepository;
        }
        public async Task<ResponseDto<object>> AddOrder(CreateOrderDto createOrderDto)
        {
            var checkValidation = await _validationRules.ValidateAsync(createOrderDto);
            if (!checkValidation.IsValid)
            {
                return new ResponseDto<object>
                {
                    Data = null,
                    Success = false,
                    ErrorCode = ErrorCodes.VALIDATION_ERROR,
                    Message = "BAŞARISZ İŞLEM!!!"
                };
            }
            var result = _mapper.Map<Order>(createOrderDto);

            result.CreatedAt = DateTime.Now;
            result.Status = OrderStatus.ALINDI;
            decimal totalPrice = 0;
            foreach (var item in result.OrderItems)
            {

                item.MenuItem = await _menuItemRepository.GetByIdAsync(item.MenuItemId);
                item.Price = item.MenuItem.Price * item.Quantity;
                totalPrice += item.Price;
            }
            result.TotalPrice = totalPrice;
            await _genericRepository.CreateAsync(result);
            return new ResponseDto<object>
            {
                Data = result,
                Success = true,
                Message = "Yeni order eklendi."
            };
        }



        public async Task<ResponseDto<object>> DeleteOrder(int id)
        {

            var itemToDelete = await _genericRepository.GetByIdAsync(id);
            if (itemToDelete == null)
            {
                return new ResponseDto<object>
                {
                    Data = null,
                    Success = false,
                    Message = "Order bulunamadi!!",
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS
                };
            }
            await _genericRepository.DeleteAsync(itemToDelete);
            return new ResponseDto<object>
            {
                Data = null,
                Success = true,
                Message = "Order kaldırıldı."
            };

        }



        public async Task<ResponseDto<List<ResultOrderDto>>> GetAllOrder()
        {
            var allOrder = await _orderRepository.GetAllOrderWithDetailAsync();
            if (allOrder.Count == 0)
            {
                return new ResponseDto<List<ResultOrderDto>>
                {
                    Data = null,
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Order bulunamadi!!"
                };
            }
            var result = _mapper.Map<List<ResultOrderDto>>(allOrder);
            return new ResponseDto<List<ResultOrderDto>>()
            {
                Data = result,
                Success = true
            };
        }


        public async Task<ResponseDto<DetailOrderDto>> GetOrderById(int id)
        {
            var orderById = await _orderRepository.GetOrderWithDetailAsync(id);
            if (orderById == null)
            {
                return new ResponseDto<DetailOrderDto>
                {
                    Data = null,
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Order bulunamadi!!"
                };
            }
            var result = _mapper.Map<DetailOrderDto>(orderById);
            return new ResponseDto<DetailOrderDto>()
            {
                Data = result,
                Success = true
            };

        }
        public async Task<ResponseDto<object>> UpdateOrder(UpdateOrderDto updateOrderDto)
        {

            var validationCheck = await _validationRules1.ValidateAsync(updateOrderDto);
            if (!validationCheck.IsValid)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    Data = null,
                    ErrorCode = ErrorCodes.VALIDATION_ERROR,
                    Message = "Başarısız işlem!!!"
                };
            }
            var itemToUpdate = await _genericRepository.GetByIdAsync(updateOrderDto.Id);
            if (itemToUpdate == null)
            {
                return new ResponseDto<object>
                {
                    Data = null,
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Order bulunamadi!!"
                };
            }
            var result = _mapper.Map(updateOrderDto, itemToUpdate);
            result.UpdatedAt = DateTime.Now;
            decimal totalPrice = 0;
            result.Status = OrderStatus.GUNCELLENDI;
            foreach (var item in result.OrderItems)
            {
                item.MenuItem = await _menuItemRepository.GetByIdAsync(item.MenuItemId);
                item.Price = item.MenuItem.Price * item.Quantity;
                totalPrice += item.Price;
            }
            result.TotalPrice = totalPrice;
            await _genericRepository.UpdateAsync(result);
            return new ResponseDto<object>
            {
                Data = result,
                Success = true,
                Message = "Order güncellendi."
            };
        }




        public async Task<ResponseDto<object>> UpdateOrderStatusToTeslimEdildiById(int id)
        {

            var statusOrderToUpdate = await _orderRepository.UpdateOrderStatusToTeslimEdildi(id);
            if (statusOrderToUpdate == null)
            {
                return new ResponseDto<object>
                {
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Siparişiniz bulunamadi.",
                    Data = null,
                    Success = false
                };
            }
            return new ResponseDto<object>
            {
                Success = false,
                Data = statusOrderToUpdate,
                Message = "Siparişiniz teslim edildi."
            };

        }



        public async Task<ResponseDto<object>> UpdateOrderStatusToUcretOdendi(int id)
        {

            var statusOrderToUpdate = await _orderRepository.UpdateOrderStatusToUcretOdendi(id);
            if (statusOrderToUpdate == null)
            {
                return new ResponseDto<object>
                {
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Siparişiniz bulunamadi.",
                    Data = null,
                    Success = false
                };
            }

            var table = await _tableRepository.GetByIdAsync(statusOrderToUpdate.TableId);
            table.IsActive = false;
            await _tableRepository.UpdateAsync(table);
            return new ResponseDto<object>
            {
                Success = true,
                Data = statusOrderToUpdate,
                Message = "Sipariş ücreti ödendi."
            };


        }


        public async Task<ResponseDto<object>> UpdateOrderStatusToHazirlaniyor(int id)
        {
            var statusOrderToUpdate = await _orderRepository.UpdateOrderStatusToHazirlaniyor(id);
            if (statusOrderToUpdate == null)
            {
                return new ResponseDto<object>
                {
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Siparişiniz bulunamadi.",
                    Data = null,
                    Success = false
                };
            }
            return new ResponseDto<object>
            {
                Success = false,
                Data = statusOrderToUpdate,
                Message = "Siparişiniz yola çıktı."
            };
        }

    }
}
