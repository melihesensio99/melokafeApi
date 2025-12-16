using KafeApi.Application.Dtos.CategoryDtos;
using KafeApi.Application.Dtos.ResponseDtos;
using KafeApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoriesWithMenuItemsAsync();
        Task<Category> GetCategoryWithMenuItemsAsync(int id);
    }
}
