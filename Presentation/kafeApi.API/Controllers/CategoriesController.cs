using KafeApi.Application.Dtos.CategoryDtos;
using KafeApi.Application.Dtos.ResponseDtos;
using KafeApi.Application.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kafeApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllCategories()
        {
            _logger.LogInformation("get-allcategories");
            var result = await _categoryService.GetAllCategories();
            _logger.LogInformation("get-allcategories" + result.Success);
            _logger.LogWarning("get-allcategories" + result.Success);
            _logger.LogError("get-allcategories" + result.Success);
            _logger.LogDebug("get-allcategories" + result.Success);
            return CreateResponse(result);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryService.GetCategoryById(id);
            return CreateResponse(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCategory(CreateCategoryDto createCategoryDto)
        {
            var result = await _categoryService.AddCategory(createCategoryDto);
            return CreateResponse(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategory(id);
            return CreateResponse(result);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            var result = await _categoryService.UpdateCategory(updateCategoryDto);
            return CreateResponse(result);
        }
        [HttpGet("getallcategorieswithmenuitems")]
        public async Task<IActionResult> GetAllCategoriesWithMenuItems()
        {
            var result = await _categoryService.GetAllCategoreisWithMenuItems();
            return CreateResponse(result);
        }
        [HttpGet("getcategorywithmenuitems")]
        public async Task<IActionResult> GetCategoryWithMenuItems(int id)
        {
            var result = await _categoryService.GetCategoryWithMenuItems(id);
            return CreateResponse(result);
        }





    }
}