using KafeApi.Application.Dtos.TableDtos;
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
    public class TableRepository : ITableRepository
    {
        private readonly AppDbContext _context;

        public TableRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Table>> GetAllActiveTablesAsync()
        {
            var result = await _context.Tables.Where(t => t.IsActive == true).ToListAsync();
            return result;
        }

        public async Task<Table> GetTableByTableNumberAsync(int tableNumber)
        {
            var result = await _context.Tables.FirstOrDefaultAsync(x => x.TableNumber == tableNumber);
            return result;
        }

        public async Task<bool> IsTableNumberExistsAsync(int tableNumber)
        {
            var check = await _context.Tables.AnyAsync(x => x.TableNumber == tableNumber);

            return check;
        }

        public async Task<Table> UpdateTableStatusByIdAsync(int id)
        {
            var result = await _context.Tables.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return null;
            }
            result.IsActive = !result.IsActive;
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<Table> UpdateTableStatusByTableNumberAsync(int tableNumber)
        {
            var result = await _context.Tables.FirstOrDefaultAsync(x => x.TableNumber == tableNumber);
            result.IsActive = !result.IsActive;
            await _context.SaveChangesAsync();
            return result;
        }

    }
}
