using AutoMapper;
using KafeApi.Application.Constants;
using KafeApi.Application.Dtos.OrderDtos;
using KafeApi.Application.Dtos.OrderItemDtos;
using KafeApi.Application.Dtos.ResponseDtos;
using KafeApi.Application.Interfaces;
using KafeApi.Application.Services.Abstract;
using KafeApi.Application.Validators.Order;
using KafeApi.Application.Validators.OrderItem;
using KafeApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Services.Concrete
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IGenericRepository<OrderItem> _genericRepository;
        private readonly IGenericRepository<MenuItem> _genericRepository2;
        private readonly IMapper _mapper;
        private readonly CreateOrderItemValidator _validationRules;
        private readonly UpdateOrderItemValidator _validationRules1;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogService<OrderItemService> _logService;


        public OrderItemService(IGenericRepository<OrderItem> genericRepository, IMapper mapper, CreateOrderItemValidator validationRules, UpdateOrderItemValidator validationRules1, IGenericRepository<MenuItem> genericRepository2, IOrderItemRepository orderItemRepository, ICacheService cacheService, ILogService<OrderItemService> logService)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _validationRules = validationRules;
            _validationRules1 = validationRules1;
            _genericRepository2 = genericRepository2;
            _orderItemRepository = orderItemRepository;
            _cacheService = cacheService;
            _logService = logService;
        }

        public async Task<ResponseDto<object>> AddOrderItem(CreateOrderItemDto createOrderItemDto)
        {

            var checkValidation = await _validationRules.ValidateAsync(createOrderItemDto);
            if (!checkValidation.IsValid)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.VALIDATION_ERROR,
                    Message = "Başarısız işlem!!!"
                };
            }

            var result = _mapper.Map<OrderItem>(createOrderItemDto);
            result.MenuItem = await _genericRepository2.GetByIdAsync(result.MenuItemId);
            result.Price = result.MenuItem.Price * result.Quantity;
            await _genericRepository.CreateAsync(result);
            _cacheService.Remove(CacheKeys.AllOrderItems);
            return new ResponseDto<object>
            {
                Data = result,
                Message = "OrderItem eklendi.",
                Success = true

            };
        }


        public async Task<ResponseDto<object>> DeleteOrderItem(int id)
        {

            var itemToDelete = await _genericRepository.GetByIdAsync(id);
            if (itemToDelete == null)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Başarısız işlem!!!",
                    Data = null
                };
            }
            await _genericRepository.DeleteAsync(itemToDelete);
            _cacheService.Remove(CacheKeys.AllOrderItems);
            return new ResponseDto<object>
            {
                Data = null,
                Message = "OrderItem silindi.",
                Success = true
            };

        }

        public async Task<ResponseDto<List<ResultOrderItemDto>>> GetAllOrderItem()
        {

            if (!_cacheService.TryGetValue(CacheKeys.AllOrderItems, out List<OrderItem> orderItems))
                _logService.LogInfo("Cache bulunamadi Veriler veritabanindan getiriliyor!!");
            orderItems = await _orderItemRepository.GetAllOrderItemByDetailAsync();
            if (orderItems != null && orderItems.Any())
                _cacheService.Set(CacheKeys.AllOrderItems, orderItems, options: new()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(30),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                });
            if (orderItems.Count == 0)
            {
                return new ResponseDto<List<ResultOrderItemDto>>
                {
                    Success = false,
                    Message = "OrderItems bulunamadi",
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Data = null
                };
            }
            var result = _mapper.Map<List<ResultOrderItemDto>>(orderItems);
            return new ResponseDto<List<ResultOrderItemDto>>
            {
                Data = result,
                Success = true
            };

        }

        public async Task<ResponseDto<DetailOrderItemDto>> GetOrderItemById(int id)
        {
            OrderItem orderItem = null;
            if (_cacheService.TryGetValue(CacheKeys.AllOrder, out List<OrderItem> orderItems))
            {

                orderItem = orderItems.FirstOrDefault(x => x.Id == id);
            }
            if (orderItem == null)
            {
                _logService.LogInfo("Cache bulunamadi Veriler veritabanindan getiriliyor!!");
                orderItem = await _orderItemRepository.GetOrderItemByDetailAsync(id);
            }
            if (orderItem == null)
                return new ResponseDto<DetailOrderItemDto>
                {
                    Data = null,
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Başarısız işlem!!"
                };

            var result = _mapper.Map<DetailOrderItemDto>(orderItem);
            return new ResponseDto<DetailOrderItemDto>()
            {
                Data = result,
                Success = true
            };
        }



        public async Task<ResponseDto<object>> UpdateOrderItem(UpdateOrderItemDto updateOrderItemDto)
        {

            var checkValidation = await _validationRules1.ValidateAsync(updateOrderItemDto);
            if (!checkValidation.IsValid)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.VALIDATION_ERROR,
                    Message = "Başarısız işlem!!!"
                };
            }
            var orderItemToUpdate = await _genericRepository.GetByIdAsync(updateOrderItemDto.Id);
            if (orderItemToUpdate == null)
            {
                return new ResponseDto<object>()
                {
                    Data = null,
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Başarısız işlem!!"
                };
            }

            var result = _mapper.Map(updateOrderItemDto, orderItemToUpdate);
            await _genericRepository.UpdateAsync(result);
            _cacheService.Remove(CacheKeys.AllOrderItems);
            return new ResponseDto<object>()
            {
                Success = true,
                Data = result,
            };
        }

    }
}

