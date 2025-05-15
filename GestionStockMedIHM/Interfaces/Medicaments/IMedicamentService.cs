using GestionStockMedIHM.Domain.DTOs.Medicaments;
using GestionStockMedIHM.Domain.DTOs.Responses;

namespace GestionStockMedIHM.Interfaces.Medicaments
{
    public interface IMedicamentService
    {
        Task<ApiResponse<MedicamentDto>> GetByIdAsync(int id);
        Task <ApiResponse<IEnumerable<MedicamentDto>>> GetAllAsync();
        Task <ApiResponse<MedicamentDto>> CreateAsync(CreateMedicamentDto createMedicamentDto);
        Task<ApiResponse<MedicamentDto>> UpdateAsync(int id, UpdateMedicamentDto updateMedicamentDto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<MedicamentDto>> GetByNomAsync(string nom);

    }
}
