using KafeApi.Application.Dtos.MenuItemDto;
using KafeApi.Application.Dtos.MenuItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Dtos.CategoryDtos
{
    public class DetailCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public List<ResultMenuItemWoutCategoryDto> MenuItems { get; set; }

    }
}
 