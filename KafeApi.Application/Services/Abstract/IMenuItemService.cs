using KafeApi.Application.Dtos.CategoryDtos;
using KafeApi.Application.Dtos.MenuItemDto;
using KafeApi.Application.Dtos.MenuItemDtos;
using KafeApi.Application.Dtos.MenuItemsDto;
using KafeApi.Application.Dtos.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Services.Abstract
{
    public interface IMenuItemService
    {
        Task<ResponseDto<List<ResultMenuItemDto>>> GetAllMenuItem();
        Task<ResponseDto<DetailMenuItemDto>> GetMenuItemById(int id);
        Task<ResponseDto<object>> AddMenuItem(CreateMenuItemDto createMenuItemDto);
        Task<ResponseDto<object>> UpdateMenuItem(UpdateMenuItemDto updateMenuItemDto);
        Task<ResponseDto<object>> DeleteMenuItem(int id);
    }
}
