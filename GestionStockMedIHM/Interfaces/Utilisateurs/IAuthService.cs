using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Domain.DTOs.Utilisateurs;
using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Interfaces.Utilisateurs
{
    public interface IAuthService
    {
        Task<ApiResponse<UtilisateurResponseDto>> RegisterAsync(UtilisateurRegisterDto utilisateurRegisterDto);
        Task<ApiResponse<string>> LoginAsync(UtilisateurLoginDto utilisateurLoginDto);
        Task<ApiResponse<UtilisateurResponseDto>> UpdateUtilisateurEtatAsync(int IdUtilisateur, UtilisateurUpdateEtatDto utilisateurUpdateEtatDto);
        string GenerateJwtToken(Utilisateur utilisateur);

    }
}
