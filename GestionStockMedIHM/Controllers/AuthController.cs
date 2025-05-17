using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Domain.DTOs.Utilisateurs;
using GestionStockMedIHM.Interfaces.Utilisateurs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionStockMedIHM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UtilisateurResponseDto>>> Register([FromBody] UtilisateurRegisterDto utilisateurRegisterDto)
        {
            var response = await _authService.RegisterAsync(utilisateurRegisterDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public  async Task<ActionResult<ApiResponse<string>>> Login([FromBody] UtilisateurLoginDto utilisateurLoginDto)
        {
            var response = await _authService.LoginAsync(utilisateurLoginDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPut("users/{id}/etat")]
        public async Task<ActionResult<ApiResponse<UtilisateurResponseDto>>> UpdateUtilisateurEtat(int id, [FromBody] UtilisateurUpdateEtatDto utilisateurUpdateEtatDto)
        {
            var response = await _authService.UpdateUtilisateurEtatAsync(id, utilisateurUpdateEtatDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
