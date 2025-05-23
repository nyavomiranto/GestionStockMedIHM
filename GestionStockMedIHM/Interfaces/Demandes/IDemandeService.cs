using GestionStockMedIHM.Domain.DTOs.Demandes;
using GestionStockMedIHM.Domain.DTOs.EntreStocks;
using GestionStockMedIHM.Domain.DTOs.Responses;

namespace GestionStockMedIHM.Interfaces.Demandes
{
    public interface IDemandeService
    {
        Task<ApiResponse<DemandeResponseDto>> GetByIdAsync(int id);
        Task<ApiResponse<IEnumerable<DemandeResponseDto>>> GetAllAsync();
        Task<ApiResponse<DemandeResponseDto>> CreateAsync(CreateDemandeDto createDemandeDto);
        Task<ApiResponse<DemandeResponseDto>> UpdateAsync(int id, UpdateDemandeDto updateDemandeDto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<string>> ConfirmDemandeAsync(int demandeId);
        Task<ApiResponse<string>> RefuseDemandeAsync(int demandeId);
    }
}
