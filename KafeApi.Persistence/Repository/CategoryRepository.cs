using KafeApi.Application.Interfaces;
using KafeApi.Domain.Entities;
using KafeApi.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Persistence.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategoriesWithMenuItemsAsync()
        {
           var result = await _context.Categories.Include(c => c.MenuItems).ToListAsync();  
            return result;
        }

        public async Task<Category> GetCategoryWithMenuItemsAsync(int id)
        {
            var result = await _context.Categories.Include(c => c.MenuItems).FirstOrDefaultAsync(x=>x.Id == id);
            return result;
        }
    }
}
