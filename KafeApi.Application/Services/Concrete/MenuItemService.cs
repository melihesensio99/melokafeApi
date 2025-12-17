using AutoMapper;
using KafeApi.Application.Dtos.MenuItemDto;
using KafeApi.Application.Dtos.MenuItemDtos;
using KafeApi.Application.Dtos.MenuItemsDto;
using KafeApi.Application.Dtos.ResponseDtos;
using KafeApi.Application.Interfaces;
using KafeApi.Application.Services.Abstract;
using KafeApi.Application.Validators.MenuItem;
using KafeApi.Domain.Entities;
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

        public MenuItemService(IGenericRepository<MenuItem> genericRepository, IMapper mapper, AddMenuItemValidator validation, UpdateMenuItemValidator validationn, IGenericRepository<Category> genericRepositoryCategory)
        {
            _validation = validation;
            _validationn = validationn;
            _genericRepository = genericRepository;
            _mapper = mapper;
            _genericRepositoryCategory = genericRepositoryCategory;
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
            return new ResponseDto<object>
            {
                Success = true,
                Data = null,
                Message = "MenuItem silindi."
            };
        }


        public async Task<ResponseDto<List<ResultMenuItemDto>>> GetAllMenuItem()
        {
            var allMenuItems = await _genericRepository.GetAllAsync();
            var listCategory = await _genericRepositoryCategory.GetAllAsync();
            if (allMenuItems.Count == 0)
            {
                return new ResponseDto<List<ResultMenuItemDto>>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "MenuItems bulunamadi.",
                    Data = null
                };
            }
            var result = _mapper.Map<List<ResultMenuItemDto>>(allMenuItems);
            return new ResponseDto<List<ResultMenuItemDto>>
            {
                Success = true,
                Data = result
            };

        }


        public async Task<ResponseDto<DetailMenuItemDto>> GetMenuItemById(int id)
        {

            var menuItem = await _genericRepository.GetByIdAsync(id);
            if (menuItem == null)
            {
                return new ResponseDto<DetailMenuItemDto>
                {
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Success = false,
                    Message = "MenuItem not found.",
                    Data = null
                };
            }
            var result = _mapper.Map<DetailMenuItemDto>(menuItem);
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
            return new ResponseDto<object>
            {
                Data = result,
                Success = true,
                Message = "MenuItem güncellendi.",
            };
        }
    }
}



