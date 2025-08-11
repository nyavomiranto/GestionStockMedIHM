using GestionStockMedIHM.Domain.DTOs.Demandes;
using GestionStockMedIHM.Domain.DTOs.SortieStocks;
using GestionStockMedIHM.Interfaces.SortieStocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionStockMedIHM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class SortieStockController : ControllerBase
    {
        private readonly ISortieStockService _sortieStockService;

        public SortieStockController(ISortieStockService sortieStockService)
        {
            _sortieStockService = sortieStockService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SortieStockResponseDto>>> GetAll()
        {
            var response = await _sortieStockService.GetAllAsync();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
