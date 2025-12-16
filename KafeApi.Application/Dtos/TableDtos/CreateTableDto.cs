using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Dtos.TableDtos
{
    public class CreateTableDto
    {
        public int TableNumber { get; set; }
        public bool IsActive { get; set; }
        public int TableCapacity { get; set; }
    }
}
