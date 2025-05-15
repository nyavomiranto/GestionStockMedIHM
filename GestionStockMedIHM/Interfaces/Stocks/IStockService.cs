using GestionStockMedIHM.Domain.DTOs.EntreStocks;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Domain.DTOs.Stocks;
using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Interfaces.Stocks
{
    public interface IStockService
    {
        Task<ApiResponse<StockResponseDto>> GetByIdAsync(int id);
        Task<ApiResponse<IEnumerable<StockResponseDto>>> GetAllAsync();
        Task<ApiResponse<StockResponseDto>> CreateAsync(CreateStockDto createStockDto);
        Task<ApiResponse<StockResponseDto>> UpdateAsync(int id, UpdateStockDto updateStockDto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<StockResponseDto>> GetByMedicamentAndDateRepemptionAsync(int medicamentId, DateTime datePeremption);
    }
}
