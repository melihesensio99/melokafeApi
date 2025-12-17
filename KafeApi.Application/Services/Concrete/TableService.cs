using AutoMapper;
using KafeApi.Application.Dtos.ResponseDtos;
using KafeApi.Application.Dtos.TableDtos;
using KafeApi.Application.Interfaces;
using KafeApi.Application.Services.Abstract;
using KafeApi.Application.Validators.Table;
using KafeApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KafeApi.Application.Services.Concrete
{
    public class TableService : ITableService
    {
        private readonly IGenericRepository<Table> _genericRepository;
        private readonly IMapper _mapper;
        private readonly AddTableValidator _addvalidation;
        private readonly UpdateTableValidator _updatevalidation;
        private readonly ITableRepository _tableRepository;

        public TableService(IGenericRepository<Table> genericRepository, IMapper mapper, AddTableValidator addvalidation, UpdateTableValidator updatevalidation, ITableRepository tableRepository)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _addvalidation = addvalidation;
            _updatevalidation = updatevalidation;
            _tableRepository = tableRepository;
        }

        public async Task<ResponseDto<object>> AddTable(CreateTableDto createTableDto)
        {

            var checkValidation = await _addvalidation.ValidateAsync(createTableDto);
            if (!checkValidation.IsValid)
            {
                return new ResponseDto<object>
                {
                    ErrorCode = ErrorCodes.VALIDATION_ERROR,
                    Data = null,
                    Message = "Başarısız işlem!!!",
                    Success = false
                };
            }
            var checkTableNumber = await _tableRepository.IsTableNumberExistsAsync(createTableDto.TableNumber);
            if (checkTableNumber)
            {
                return new ResponseDto<object>
                {
                    ErrorCode = ErrorCodes.CONFLICT,
                    Message = "TableNumber mevcut!!!",
                    Success = false
                };
            }

            var result = _mapper.Map<Table>(createTableDto);
            await _genericRepository.CreateAsync(result);

            return new ResponseDto<object>
            {
                Data = result,
                Success = true,
                Message = "Table başarıyla eklendi."
            };
        }


        public async Task<ResponseDto<object>> DeleteTable(int id)
        {
            var itemToDelete = await _genericRepository.GetByIdAsync(id);
            if (itemToDelete == null)
            {
                return new ResponseDto<object>
                {
                    Data = null,
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS
                };
            }
            await _genericRepository.DeleteAsync(itemToDelete);
            return new ResponseDto<object>
            {
                Success = true,
                Message = "Table başarıyla kaldırıldı."
            };

        }


        public async Task<ResponseDto<List<ResultTableDto>>> GetAllActiveTables()
        {
            var activeTables = await _tableRepository.GetAllActiveTablesAsync();
            if (activeTables.Count == 0)
            {
                return new ResponseDto<List<ResultTableDto>>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Başarısız işlem!!!",
                    Data = null
                };
            }
            var result = _mapper.Map<List<ResultTableDto>>(activeTables);
            return new ResponseDto<List<ResultTableDto>>
            {
                Success = true,
                Data = result,
            };
        }


        public async Task<ResponseDto<List<ResultTableDto>>> GetAllTables()
        {

            var allTables = await _genericRepository.GetAllAsync();
            if (allTables.Count == 0)
            {
                return new ResponseDto<List<ResultTableDto>>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Başarısız işlem!!!",
                    Data = null
                };
            }
            var result = _mapper.Map<List<ResultTableDto>>(allTables);
            return new ResponseDto<List<ResultTableDto>>
            {
                Data = result,
                Success = true,
            };
        }

        public async Task<ResponseDto<DetailTableDto>> GetTableById(int id)
        {

            var table = await _genericRepository.GetByIdAsync(id);
            if (table == null)
            {
                return new ResponseDto<DetailTableDto>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Başarısız işlem!!!"
                };
            }
            var result = _mapper.Map<DetailTableDto>(table);
            return new ResponseDto<DetailTableDto>
            {
                Success = true,
                Data = result,
            };
        }

        public async Task<ResponseDto<DetailTableDto>> GetTableByTableNumber(int tableNumber)
        {

            var number = await _tableRepository.GetTableByTableNumberAsync(tableNumber);
            if (number == null)
            {
                return new ResponseDto<DetailTableDto>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Başarısız işlem!!!",
                    Data = null
                };
            }
            var result = _mapper.Map<DetailTableDto>(number);
            return new ResponseDto<DetailTableDto>
            {
                Data = result,
                Success = true
            };
        }


        public async Task<ResponseDto<object>> UpdateTable(UpdateTableDto updateTableDto)
        {

            var checkValidation = await _updatevalidation.ValidateAsync(updateTableDto);
            if (!checkValidation.IsValid)
            {
                return new ResponseDto<object>
                {
                    ErrorCode = ErrorCodes.VALIDATION_ERROR,
                    Data = null,
                    Message = "Başarısız işlem!!!",
                    Success = false
                };
            }
            var tableToUpdate = await _genericRepository.GetByIdAsync(updateTableDto.Id);
            if (tableToUpdate == null)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Bir hata oluştu!!",
                    Data = null
                };
            }
            var checkTableNumber = await _tableRepository.IsTableNumberExistsAsync(updateTableDto.TableNumber);
            if (checkTableNumber)
            {
                return new ResponseDto<object>
                {
                    Data = null,
                    ErrorCode = ErrorCodes.CONFLICT,
                    Message = "TableNumber mevcut!!!"
                };
            }
            var result = _mapper.Map(updateTableDto, tableToUpdate);
            await _genericRepository.UpdateAsync(result);
            return new ResponseDto<object>
            {
                Success = true,
                Data = result,
                Message = "Table başarıyla güncellendi."
            };
        }

        public async Task<ResponseDto<object>> UpdateTableStatusById(int id)
        {

            var updateStatusById = await _tableRepository.UpdateTableStatusByIdAsync(id);
            if (updateStatusById == null)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Başarısız işlem!!!",
                    Data = null
                };
            }
            return new ResponseDto<object>
            {
                Success = true,
                Message = "Status başarıyla güncellendi.",
            };

        }

        public async Task<ResponseDto<object>> UpdateTableStatusByTableNumber(int tableNumber)
        {

            var updateStatusByTableNumber = await _tableRepository.UpdateTableStatusByTableNumberAsync(tableNumber);
            if (updateStatusByTableNumber == null)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NOT_FOUND_STATUS,
                    Message = "Başarısız işlem!!!",
                    Data = null
                };
            }
            return new ResponseDto<object>
            {
                Success = true,
                Message = "Status başarıyla güncellendi.",
            };
        }

    }
}
