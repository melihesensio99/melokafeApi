using AutoMapper;
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


        public OrderItemService(IGenericRepository<OrderItem> genericRepository, IMapper mapper, CreateOrderItemValidator validationRules, UpdateOrderItemValidator validationRules1, IGenericRepository<MenuItem> genericRepository2, IOrderItemRepository orderItemRepository)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _validationRules = validationRules;
            _validationRules1 = validationRules1;
            _genericRepository2 = genericRepository2;
            _orderItemRepository = orderItemRepository;
        }

        public async Task<ResponseDto<object>> AddOrderItem(CreateOrderItemDto createOrderItemDto)
        {
            try
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
                return new ResponseDto<object>
                {
                    Data = result,
                    Message = "OrderItem eklendi.",
                    Success = true
                };
            }
            catch (Exception)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.EXCEPTION,
                    Message = "Başarısız işlem!!!"
                };
            }
        }

        public async Task<ResponseDto<object>> DeleteOrderItem(int id)
        {
            try
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
                return new ResponseDto<object>
                {
                    Data = null,
                    Message = "OrderItem silindi.",
                    Success = true
                };
            }
            catch (Exception)
            {

                return new ResponseDto<object>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.EXCEPTION,
                    Message = "Başarısız işlem!!!"
                };
            }
        }

        public async Task<ResponseDto<List<ResultOrderItemDto>>> GetAllOrderItem()
        {
            try
            {
                var allOrderItem = await _orderItemRepository.GetAllOrderItemByDetailAsync();
                if (allOrderItem.Count == 0)
                {
                    return new ResponseDto<List<ResultOrderItemDto>>
                    {
                        Data = null,
                        Message = "Başarısız işlem!!!",
                        Success = false,
                        ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    };
                }
                var result = _mapper.Map<List<ResultOrderItemDto>>(allOrderItem);
                return new ResponseDto<List<ResultOrderItemDto>>
                {
                    Data = result,
                    Success = true
                };
            }
            catch (Exception)
            {
                return new ResponseDto<List<ResultOrderItemDto>>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.EXCEPTION,
                    Message = "Başarısız işlem!!!"
                };
            }
        }

        public async Task<ResponseDto<DetailOrderItemDto>> GetOrderItemById(int id)
        {
            try
            {
                var orderItemById = await _orderItemRepository.GetOrderItemByDetailAsync(id);
                if (orderItemById == null)
                {
                    return new ResponseDto<DetailOrderItemDto>
                    {
                        Data = null,
                        Success = false,
                        ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                        Message = "Başarısız işlem!!"
                    };
                }
                var result = _mapper.Map<DetailOrderItemDto>(orderItemById);
                return new ResponseDto<DetailOrderItemDto>()
                {
                    Data = result,
                    Success = true
                };
            }
            catch (Exception)
            {
                return new ResponseDto<DetailOrderItemDto>()
                {
                    Success = false,
                    ErrorCode = ErrorCodes.EXCEPTION,
                    Message = "Başarısız işlem!!!"
                };
            }
        }

        public async Task<ResponseDto<object>> UpdateOrderItem(UpdateOrderItemDto updateOrderItemDto)
        {
            try
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
                return new ResponseDto<object>()
                {
                    Success = true,
                    Data = result,
                };
            }
            catch (Exception)
            {
                return new ResponseDto<object>()
                {
                    Success = false,
                    ErrorCode = ErrorCodes.EXCEPTION,
                    Message = "Başarısız işlem!!!"
                };
            }
        }
    }
}
