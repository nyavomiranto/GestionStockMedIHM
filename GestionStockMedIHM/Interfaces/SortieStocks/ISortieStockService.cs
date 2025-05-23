using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Domain.DTOs.SortieStocks;

namespace GestionStockMedIHM.Interfaces.SortieStocks
{
    public interface ISortieStockService
    {
        Task<ApiResponse<SortieStockResponseDto>> CreateSortieStockAsync(CreateSortieStockDto dto);
        Task<ApiResponse<IEnumerable<SortieStockResponseDto>>> GetAllAsync();
    }
}
