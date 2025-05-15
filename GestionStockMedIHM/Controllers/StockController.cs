using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Domain.DTOs.Stocks;
using GestionStockMedIHM.Interfaces.Stocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionStockMedIHM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StockResponseDto>>> GetById(int id)
        {
            var response = await _stockService.GetByIdAsync(id);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ApiResponse<StockResponseDto>>>>> GetAll()
        {
            var response = await _stockService.GetAllAsync();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<StockResponseDto>>> Create([FromBody] CreateStockDto stock)
        {
            var response = await _stockService.CreateAsync(stock);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<StockResponseDto>>> Update(int id, [FromBody] UpdateStockDto stock)
        {
            var response = await _stockService.UpdateAsync(id, stock);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var response = await _stockService.DeleteAsync(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        } 
    }
}
