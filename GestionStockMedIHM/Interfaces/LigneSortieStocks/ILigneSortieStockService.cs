using GestionStockMedIHM.Domain.DTOs.LignesDemandes;
using GestionStockMedIHM.Domain.DTOs.LigneSortieStocks;
using GestionStockMedIHM.Domain.DTOs.Responses;

namespace GestionStockMedIHM.Interfaces.LigneSortieStocks
{
    public interface ILigneSortieStockService
    {
        Task<ApiResponse<List<LigneSortieStockResponseDto>>> CreateLignesSortieStockAsync(int sortieStockId, List<CreateLigneSortieStockDto> ligneSortieStockDtos);
    }
}

