using GestionStockMedIHM.Domain.DTOs.Medicaments;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Interfaces.Medicaments;
using GestionStockMedIHM.Models.Entities;
using GestionStockMedIHM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionStockMedIHM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicamentController : ControllerBase
    {
        private readonly IMedicamentService _medicamentService;

        public MedicamentController(IMedicamentService mediicamentService)
        {
            _medicamentService = mediicamentService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MedicamentDto>>> GetById (int id)
        {
            var response = await _medicamentService.GetByIdAsync (id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<MedicamentDto>>>> GetAll()
        {
            var response = await _medicamentService.GetAllAsync();
            if (!response.Success) 
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<MedicamentDto>>> Create([FromBody] CreateMedicamentDto medicament)
        {
            var response = await _medicamentService.CreateAsync(medicament);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<MedicamentDto>>> Update(int id, [FromBody] UpdateMedicamentDto medicament)
        {
            var response = await _medicamentService.UpdateAsync(id, medicament);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var response = await _medicamentService.DeleteAsync(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
