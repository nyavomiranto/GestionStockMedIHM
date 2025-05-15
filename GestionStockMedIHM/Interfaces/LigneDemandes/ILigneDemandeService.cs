using GestionStockMedIHM.Domain.DTOs.Fournisseurs;
using GestionStockMedIHM.Domain.DTOs.LignesDemandes;
using GestionStockMedIHM.Domain.DTOs.Responses;

namespace GestionStockMedIHM.Interfaces.LigneDemandes
{
    public interface ILigneDemandeService
    {
        Task<ApiResponse<LigneDemandeResponseDto>> CreateAsync(CreateLigneDemandeDto createLigneDemandeDto, int DemandeId);
    }
}
