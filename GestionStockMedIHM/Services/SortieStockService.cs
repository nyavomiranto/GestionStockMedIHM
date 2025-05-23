using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Demandes;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Domain.DTOs.SortieStocks;
using GestionStockMedIHM.Interfaces.Demandes;
using GestionStockMedIHM.Interfaces.LigneSortieStocks;
using GestionStockMedIHM.Interfaces.SortieStocks;
using GestionStockMedIHM.Models.Entities;
using GestionStockMedIHM.Repositories;

namespace GestionStockMedIHM.Services
{
    public class SortieStockService: ISortieStockService
    {
        private readonly ISortieStockRepository _sortieStockRepository;
        private readonly ILigneSortieStockService _ligneSortieStockService;
        private readonly IMapper _mapper;
        private readonly ILogger<SortieStockService> _logger;

        public SortieStockService(ISortieStockRepository sortieStockRepository, ILigneSortieStockService ligneSortieStockService, IMapper mapper, ILogger<SortieStockService> logger)
        {
            _sortieStockRepository = sortieStockRepository;
            _ligneSortieStockService = ligneSortieStockService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<SortieStockResponseDto>> CreateSortieStockAsync(CreateSortieStockDto createSortieStockDto)
        {
            try
            {
                var sortieStock = _mapper.Map<SortieStock>(createSortieStockDto);
                sortieStock.UtilisateurId = createSortieStockDto.UtilisateurId;

                await _sortieStockRepository.AddAsync(sortieStock);

                var ligneSortieStockResult = await _ligneSortieStockService.CreateLignesSortieStockAsync(sortieStock.Id, createSortieStockDto.LignesSortieStock);

                if (!ligneSortieStockResult.Success)
                {
                    return ApiResponse<SortieStockResponseDto>.ErrorResponse(
                        "Erreur lors de la création des lignes de sortie de stock",
                        ligneSortieStockResult.Errors);
                }

                var responseDto = _mapper.Map<SortieStockResponseDto>(sortieStock);
                responseDto.LignesSorties = ligneSortieStockResult.Data;

                return ApiResponse<SortieStockResponseDto>.SuccessResponse(responseDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<SortieStockResponseDto>.ErrorResponse(
                    "Erreur lors de la création de la sortie de stock",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<IEnumerable<SortieStockResponseDto>>> GetAllAsync()
        {
            try
            {
                var sorties = await _sortieStockRepository.GetAllWithDetailsAsync();
                var result = _mapper.Map<IEnumerable<SortieStockResponseDto>>(sorties);
                return ApiResponse<IEnumerable<SortieStockResponseDto>>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SortieStockResponseDto>>.ErrorResponse(
                   "Erreur lors de la recupération des sorties de stocks",
                   new List<string> { ex.Message });
            }

        }

    }
}
