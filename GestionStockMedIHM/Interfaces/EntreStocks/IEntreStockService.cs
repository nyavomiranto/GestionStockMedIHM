using GestionStockMedIHM.Domain.DTOs.EntreStocks;
using GestionStockMedIHM.Domain.DTOs.Responses;

namespace GestionStockMedIHM.Interfaces.EntreStocks
{
    public interface IEntreStockService
    {
        Task<ApiResponse<EntreStockResponseDto>> GetByIdAsync(int id);
        Task<ApiResponse<IEnumerable<EntreStockResponseDto>>> GetAllAsync();
        Task<ApiResponse<EntreStockResponseDto>> CreateAsync(CreateEntreStockDto createEntreStockDto);
        Task<ApiResponse<EntreStockResponseDto>> UpdateAsync(int id, UpdateEntreStockDto updateEntreStockDto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
