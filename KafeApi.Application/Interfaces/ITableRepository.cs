using KafeApi.Application.Dtos.TableDtos;
using KafeApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Interfaces
{
    public interface ITableRepository 
    {
        Task<Table> GetTableByTableNumberAsync(int tableNumber);
         Task<List<Table>> GetAllActiveTablesAsync();
        Task<Table> UpdateTableStatusByIdAsync(int id);
        Task<Table> UpdateTableStatusByTableNumberAsync(int tableNumber);
        Task<bool> IsTableNumberExistsAsync(int tableNumber);

    }
}
