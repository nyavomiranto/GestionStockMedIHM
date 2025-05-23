using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.LigneSortieStocks;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Domain.Entities;
using GestionStockMedIHM.Interfaces.LigneSortieStocks;
using GestionStockMedIHM.Interfaces.Medicaments;
using GestionStockMedIHM.Interfaces.Stocks;
using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;
using System.Transactions;

namespace GestionStockMedIHM.Services
{
    public class LigneSortieStockService : ILigneSortieStockService
    {
        private readonly ILigneSortieStockRepository _ligneSortieStockRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LigneSortieStockService> _logger;
        private readonly AppDbContext _appDbContext;

        public LigneSortieStockService(ILigneSortieStockRepository ligneSortieStockRepository, IMapper mapper, IStockRepository stockRepository, ILogger<LigneSortieStockService> logger, AppDbContext appDbContext)
        {
            _ligneSortieStockRepository = ligneSortieStockRepository;
            _stockRepository = stockRepository;
            _mapper = mapper;
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public async Task<ApiResponse<List<LigneSortieStockResponseDto>>> CreateLignesSortieStockAsync(int sortieStockId, List<CreateLigneSortieStockDto> ligneSortieStockDtos)
        {
            try
            {
                if (ligneSortieStockDtos == null || !ligneSortieStockDtos.Any())
                {
                    return ApiResponse<List<LigneSortieStockResponseDto>>.ErrorResponse("La liste des lignes de sortie est vide ou nulle.");
                }

                var ligneSortieStocks = _mapper.Map<List<LigneSortieStock>>(ligneSortieStockDtos);
                foreach (var ligne in ligneSortieStocks)
                {
                    ligne.SortieStockId = sortieStockId;
                }

                using var transaction = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    foreach (var ligne in ligneSortieStocks)
                    {
                        var quantiteRestante = ligne.Quantite;
                        var stocks = await _stockRepository.GetStocksByMedicamentIdAsync(ligne.MedicamentId);

                        if (stocks == null || !stocks.Any() || stocks.Sum(s => s.Quantite) < quantiteRestante)
                        {
                            return ApiResponse<List<LigneSortieStockResponseDto>>.ErrorResponse(
                                $"Stock insuffisant pour le médicament ID {ligne.MedicamentId}");
                        }

                        foreach (var stock in stocks.OrderBy(s => s.DatePeremption))
                        {
                            if (quantiteRestante <= 0) break;

                            int quantiteAReduire = Math.Min(stock.Quantite, quantiteRestante);
                            stock.Quantite -= quantiteAReduire;
                            quantiteRestante -= quantiteAReduire;

                            await _stockRepository.UpdateAsync(stock);
                        }
                    }

                    await _ligneSortieStockRepository.AddRangeAsync(ligneSortieStocks);

                    var medicamentIds = ligneSortieStocks.Select(l => l.MedicamentId).Distinct().ToList();
                    var medicaments = await _appDbContext.Medicaments
                        .Where(m => medicamentIds.Contains(m.Id))
                        .ToListAsync();

                    foreach (var ligne in ligneSortieStocks)
                    {
                        ligne.Medicament = medicaments.FirstOrDefault(m => m.Id == ligne.MedicamentId);
                        if (ligne.Medicament == null)
                        {
                            Console.WriteLine($"[WARN] Medicament ID {ligne.MedicamentId} not found for sortieStockId {sortieStockId}");
                        }
                    }

                    await transaction.CommitAsync();

                    var responseDtos = _mapper.Map<List<LigneSortieStockResponseDto>>(ligneSortieStocks);
                    return ApiResponse<List<LigneSortieStockResponseDto>>.SuccessResponse(responseDtos);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<List<LigneSortieStockResponseDto>>.ErrorResponse(
                        "Erreur lors de la création des lignes de sortie de stock",
                        new List<string> { ex.Message });
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<List<LigneSortieStockResponseDto>>.ErrorResponse(
                    "Erreur lors de la création des lignes de sortie de stock",
                    new List<string> { ex.Message });
            }
        }
    }
}
