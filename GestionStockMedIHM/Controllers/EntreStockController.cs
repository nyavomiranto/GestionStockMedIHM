using GestionStockMedIHM.Domain.DTOs.EntreStocks;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Interfaces.EntreStocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionStockMedIHM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class EntreStockController : ControllerBase
    {
        private readonly IEntreStockService _entreStockService;

        public EntreStockController(IEntreStockService entreStockService)
        {
            _entreStockService = entreStockService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EntreStockResponseDto>>> GetById (int id)
        {
            var response = await _entreStockService.GetByIdAsync (id);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<EntreStockResponseDto>>>> GetAll()
        {
            var response = await _entreStockService.GetAllAsync();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EntreStockResponseDto>>> Create([FromBody] CreateEntreStockDto entreStock)
        {
            var response = await _entreStockService.CreateAsync (entreStock);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<EntreStockResponseDto>>> Update(int id, [FromBody] UpdateEntreStockDto entreStock)
        {
            var response = await _entreStockService.UpdateAsync(id, entreStock);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var response  = await _entreStockService.DeleteAsync(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
