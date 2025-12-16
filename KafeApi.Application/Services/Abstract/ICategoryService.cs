using KafeApi.Application.Dtos.CategoryDtos;
using KafeApi.Application.Dtos.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Services.Abstract
{
    public interface ICategoryService
    {
        Task<ResponseDto<List<ResultCategoryDto>>> GetAllCategories();
        Task<ResponseDto<DetailCategoryDto>> GetCategoryById(int id);
        Task<ResponseDto<object>> AddCategory(CreateCategoryDto createCategoryDto);
        Task<ResponseDto<object>> UpdateCategory(UpdateCategoryDto update);
        Task<ResponseDto<object>> DeleteCategory(int id);
        Task<ResponseDto<DetailCategoryDto>> GetCategoryWithMenuItems(int id);
        Task<ResponseDto<List<ResultCategoryWithMenuItems>>> GetAllCategoreisWithMenuItems();



    }
}
