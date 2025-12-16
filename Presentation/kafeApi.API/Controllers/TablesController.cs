using KafeApi.Application.Dtos.ResponseDtos;
using KafeApi.Application.Dtos.TableDtos;
using KafeApi.Application.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace kafeApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : BaseController
    {
        private readonly ITableService _tableService;

        public TablesController(ITableService tableService)
        {
            _tableService = tableService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTablesAsync()
        {
            var result = await _tableService.GetAllTables();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTableById(int id)
        {
            var result = await _tableService.GetTableById(id);
            return CreateResponse(result);
        }
        [HttpGet("gettablebytablenumber")]
        public async Task<IActionResult> GetTableByTableNumber(int tableNumber)
        {
            var result = await _tableService.GetTableByTableNumber(tableNumber);
            return CreateResponse(result);
        }
        [HttpGet("getallactivetables")]
        public async Task<IActionResult> GetAllActiveTables()
        {
            var result = await _tableService.GetAllActiveTables();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddTable(CreateTableDto createTableDto)
        {
            var result = await _tableService.AddTable(createTableDto);
            return CreateResponse(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var result = await _tableService.DeleteTable(id);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTable(UpdateTableDto updateTableDto)
        {
            var result = await _tableService.UpdateTable(updateTableDto);
            return CreateResponse(result);
        }
        [HttpPut("updatetablesatusbyid")]
        public async Task<IActionResult> UpdateTableStatusById(int id)
        {
            var result = await _tableService.UpdateTableStatusById(id);
            return CreateResponse(result);

        }
        [HttpPut("updatetablestatusbytablenumber")]
        public async Task<IActionResult> UpdateTableStatusByTableNumber(int tableNumber)
        {
            var result = await _tableService.UpdateTableStatusByTableNumber(tableNumber);
            return CreateResponse(result);
            

        }
    }



}

