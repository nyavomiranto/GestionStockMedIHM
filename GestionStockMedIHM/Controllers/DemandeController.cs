using GestionStockMedIHM.Domain.DTOs.Demandes;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Interfaces.Demandes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionStockMedIHM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Vendeur")]
    public class DemandeController : ControllerBase
    {
        private readonly IDemandeService _demandeService;

        public DemandeController(IDemandeService demandeService)
        {
            _demandeService = demandeService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DemandeResponseDto>>> GetById(int id)
        {
            var response = await _demandeService.GetByIdAsync(id);
            if(response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DemandeResponseDto>>> GetAll()
        {
            var response = await _demandeService.GetAllAsync();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<DemandeResponseDto>>> Create([FromBody] CreateDemandeDto demande)
        {
            var response = await _demandeService.CreateAsync(demande);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<DemandeResponseDto>>> Update(int id, [FromBody] UpdateDemandeDto demande)
        {
            var response = await _demandeService.UpdateAsync(id, demande);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var response = await _demandeService.DeleteAsync(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("{demandeId}/confirm")]
        public async Task<ActionResult<ApiResponse<DemandeResponseDto>>> ConfirmDemandeAsync(int demandeId)
        {
            var result = await _demandeService.ConfirmDemandeAsync(demandeId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("{demandeId}/refuse")]
        public async Task<ActionResult<ApiResponse<DemandeResponseDto>>> RefuseDemandeAsync(int demandeId)
        {
            var result = await _demandeService.RefuseDemandeAsync(demandeId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
