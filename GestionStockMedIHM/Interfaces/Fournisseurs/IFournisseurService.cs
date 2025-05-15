using GestionStockMedIHM.Domain.DTOs.Fournisseurs;
using GestionStockMedIHM.Domain.DTOs.Responses;

namespace GestionStockMedIHM.Interfaces.Fournisseurs
{
    public interface IFournisseurService
    {
        Task<ApiResponse<FournisseurResponseDto>> GetByIdAsync(int id);
        Task<ApiResponse<IEnumerable<FournisseurResponseDto>>> GetAllAsync();
        Task<ApiResponse<FournisseurResponseDto>> CreateAsync(CreateFournisseurDto createFournisseurDto);
        Task<ApiResponse<FournisseurResponseDto>> UpdateAsync(int id, UpdateFournisseurDto updateFournisseurDto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<FournisseurResponseDto>> GetByNomAsync(string nom);

    }
}
