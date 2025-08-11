using GestionStockMedIHM.Domain.DTOs.Fournisseurs;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Interfaces.Fournisseurs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionStockMedIHM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class FournisseurController : ControllerBase
    {
        private readonly IFournisseurService _fournisseurService;

        public FournisseurController(IFournisseurService fournisseurService)
        {
            _fournisseurService = fournisseurService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<FournisseurResponseDto>>> GetById(int id)
        {
            var response = await _fournisseurService.GetByIdAsync(id);
            if (response == null) 
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<FournisseurResponseDto>>>> GetAll()
        {
            var response = await _fournisseurService.GetAllAsync();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<FournisseurResponseDto>>> Create([FromBody] CreateFournisseurDto fournisseur)
        {
            var response = await _fournisseurService.CreateAsync(fournisseur);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<FournisseurResponseDto>>> Update(int id, [FromBody] UpdateFournisseurDto fournisseur)
        {
            var response = await _fournisseurService.UpdateAsync(id, fournisseur);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var response = await _fournisseurService.DeleteAsync(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
