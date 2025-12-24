using AutoMapper;
using KafeApi.Application.Constants;
using KafeApi.Application.Dtos.MenuItemDto;
using KafeApi.Application.Dtos.MenuItemDtos;
using KafeApi.Application.Dtos.MenuItemsDto;
using KafeApi.Application.Dtos.ResponseDtos;
using KafeApi.Application.Interfaces;
using KafeApi.Application.Services.Abstract;
using KafeApi.Application.Validators.MenuItem;
using KafeApi.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Services.Concrete
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IGenericRepository<MenuItem> _genericRepository;
        private readonly IGenericRepository<Category> _genericRepositoryCategory;
        private readonly IMapper _mapper;
        private readonly AddMenuItemValidator _validation;
        private readonly UpdateMenuItemValidator _validationn;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogService<MenuItemService> _logger;
        private readonly IConfiguration _configuration;

        public MenuItemService(IGenericRepository<MenuItem> genericRepository, IMapper mapper, AddMenuItemValidator validation, UpdateMenuItemValidator validationn, IGenericRepository<Category> genericRepositoryCategory, IMemoryCache memoryCache, ILogService<MenuItemService> logger, IConfiguration configuration)
        {
            _validation = validation;
            _validationn = validationn;
            _genericRepository = genericRepository;
            _mapper = mapper;
            _genericRepositoryCategory = genericRepositoryCategory;
            _memoryCache = memoryCache;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<ResponseDto<object>> AddMenuItem(CreateMenuItemDto createMenuItemDto)
        {

            var checkValidation = await _validation.ValidateAsync(createMenuItemDto);
            if (!checkValidation.IsValid)
            {
                return new ResponseDto<object>
                {
                    Data = null,
                    ErrorCode = ErrorCodes.VALIDATION_ERROR,
                    Success = false,
                    Message = "Başarısız işlem!!"
                };
            }

            var result = _mapper.Map<MenuItem>(createMenuItemDto);
            await _genericRepository.CreateAsync(result);
            _memoryCache.Remove(CacheKeys.AllMenuItems);
            return new ResponseDto<object>
            {
                Data = result,
                Message = "MenuItem eklendi.",
                Success = true
            };
        }

        public async Task<ResponseDto<object>> DeleteMenuItem(int id)
        {

            var menuItemToDelete = await _genericRepository.GetByIdAsync(id);
            if (menuItemToDelete == null)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    Data = null,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "MenuItem bulunamadi."
                };
            }
            _genericRepository.DeleteAsync(menuItemToDelete);
            _memoryCache.Remove(CacheKeys.AllMenuItems);
            return new ResponseDto<object>
            {
                Success = true,
                Data = null,
                Message = "MenuItem silindi."
            };
        }


        public async Task<ResponseDto<List<ResultMenuItemDto>>> GetAllMenuItem()
        {
            var cache = _memoryCache.TryGetValue(CacheKeys.AllMenuItems, out List<MenuItem> menuItems);
            if (!cache)
            {
                _logger.LogInfo("MenuItems cache'de bulunamadı, veritabanından getiriliyor...");
                menuItems = await _genericRepository.GetAllAsync();
                if (menuItems != null && menuItems.Any())
                {
                    var expirationMinutes = _configuration.GetValue<int>("CacheSettings:DefaultExpirationMinutes", 10);
                    _memoryCache.Set(CacheKeys.AllMenuItems, menuItems, options: new()
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(expirationMinutes),
                        SlidingExpiration = TimeSpan.FromMinutes(expirationMinutes)

                    });
                }

            }
            if (menuItems.Count == 0)
            {
                return new ResponseDto<List<ResultMenuItemDto>>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "MenuItems bulunamadi.",
                    Data = null
                };
            }
            var result = _mapper.Map<List<ResultMenuItemDto>>(menuItems);
            return new ResponseDto<List<ResultMenuItemDto>>
            {
                Success = true,
                Data = result
            };

        }


        public async Task<ResponseDto<DetailMenuItemDto>> GetMenuItemById(int id)
        {
            MenuItem menuItems = null;
            if (_memoryCache.TryGetValue(CacheKeys.AllMenuItems, out List<MenuItem> cachedMenuItems))
            {

                menuItems = cachedMenuItems.FirstOrDefault(x => x.Id == id);
            }
            if (menuItems == null)
            {
                _logger.LogInfo($"GetMenuItemById Cachede kayitli degil veritabanindan getiriliyor id : {id}");
                menuItems = await _genericRepository.GetByIdAsync(id);
            }

            if (menuItems == null)
            {
                return new ResponseDto<DetailMenuItemDto>
                {
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Success = false,
                    Message = "MenuItem not found.",
                    Data = null
                };
            }
            var result = _mapper.Map<DetailMenuItemDto>(menuItems);
            return new ResponseDto<DetailMenuItemDto>
            {
                Data = result,
                Success = true
            };
        }

        public async Task<ResponseDto<object>> UpdateMenuItem(UpdateMenuItemDto updateMenuItemDto)
        {
            var checkValidation = await _validationn.ValidateAsync(updateMenuItemDto);
            if (!checkValidation.IsValid)
            {
                return new ResponseDto<object>
                {
                    Data = null,
                    Success = false,
                    ErrorCode = ErrorCodes.VALIDATION_ERROR,
                    Message = "Başarısız işlem!!"
                };
            }
            var menuItemToUpdate = await _genericRepository.GetByIdAsync(updateMenuItemDto.Id);
            if (menuItemToUpdate == null)
            {
                return new ResponseDto<object>
                {

                    Success = false,
                    Message = "MenuItem bulunamadi",
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Data = null
                };
            }
            var result = _mapper.Map(updateMenuItemDto, menuItemToUpdate);
            await _genericRepository.UpdateAsync(result);
            _memoryCache.Remove(CacheKeys.AllMenuItems);
            return new ResponseDto<object>
            {
                Data = result,
                Success = true,
                Message = "MenuItem güncellendi.",
            };
        }
    }
}



