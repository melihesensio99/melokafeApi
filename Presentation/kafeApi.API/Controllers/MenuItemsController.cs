using KafeApi.Application.Dtos.CategoryDtos;
using KafeApi.Application.Dtos.MenuItemDto;
using KafeApi.Application.Dtos.MenuItemsDto;
using KafeApi.Application.Dtos.ResponseDtos;
using KafeApi.Application.Services.Abstract;
using KafeApi.Application.Services.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace kafeApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : BaseController
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemsController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }
        [HttpGet]

        public async Task<IActionResult> GetAllMenuItem()
        {
            var result = await _menuItemService.GetAllMenuItem();
            return Ok(result);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetMenuItemById(int id)
        {
            var result = await _menuItemService.GetMenuItemById(id);
            return CreateResponse(result);

        }

        [HttpPost]
        public async Task<IActionResult> AddMenuItem(CreateMenuItemDto createMenuItemDto)
        {
            var result = await _menuItemService.AddMenuItem(createMenuItemDto);
            return CreateResponse(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var result = await _menuItemService.DeleteMenuItem(id);
            return CreateResponse(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateMenuItemDto updateMenuItemDto)
        {
            var result = await _menuItemService.UpdateMenuItem(updateMenuItemDto);
            return CreateResponse(result);
        }
    }
}
