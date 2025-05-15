using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Domain.DTOs.Stocks;
using GestionStockMedIHM.Interfaces.Medicaments;
using GestionStockMedIHM.Interfaces.Stocks;
using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Services
{
    public class StockService: IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IMedicamentService _mediicamentService;
        private readonly IMapper _mapper;

        public StockService(IStockRepository stockRepository, IMedicamentService mediicamentService, IMapper mapper)
        {
            _stockRepository = stockRepository;
            _mediicamentService = mediicamentService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<StockResponseDto>> GetByIdAsync(int id)
        {
            try
            {
                var stock = await _stockRepository.GetByIdWithDetailsAsync(id);
                if (stock == null)
                {
                    return ApiResponse<StockResponseDto>.ErrorResponse("Stock non trouvé");
                }
                var result = _mapper.Map<StockResponseDto>(stock);
                return ApiResponse<StockResponseDto>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<StockResponseDto>.ErrorResponse(
                    "Erreur lors de la récupération du stock",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<IEnumerable<StockResponseDto>>> GetAllAsync()
        {
            try
            {
                var stocks = await _stockRepository.GetAllWithDetailsAsync();
                var result = _mapper.Map<IEnumerable<StockResponseDto>>(stocks);
                return ApiResponse<IEnumerable<StockResponseDto>>.SuccessResponse(result);
            }
            catch (Exception ex) 
            {
                return ApiResponse<IEnumerable<StockResponseDto>>.ErrorResponse(
                    "Erreur lors de la récupération des stocks",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<StockResponseDto>> CreateAsync(CreateStockDto createStockDto)
        {
            try
            {
                var medicamentResponse = await _mediicamentService.GetByNomAsync(createStockDto.NomMedicament);
                if (!medicamentResponse.Success || medicamentResponse.Data == null)
                {
                    return ApiResponse<StockResponseDto>.ErrorResponse("Médicament non trouvé");
                }

                var entity = _mapper.Map<Stock>(createStockDto);
                entity.MedicamentId = medicamentResponse.Data.Id;

                await _stockRepository.AddAsync(entity);

                var resultDto = _mapper.Map<StockResponseDto>(entity);
                return ApiResponse<StockResponseDto>.SuccessResponse(resultDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<StockResponseDto>.ErrorResponse(
                    "Erreur lors de la creation du stock",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<StockResponseDto>> UpdateAsync(int id, UpdateStockDto updateStockDto)
        {
            try
            {
                var existingStock = await _stockRepository.GetByIdAsync(id);
                if (existingStock == null)
                {
                    return ApiResponse<StockResponseDto>.ErrorResponse("Stock non trouvé");
                }

                if (!string.IsNullOrEmpty(updateStockDto.NomMedicament) && !existingStock.Medicament.Nom.Equals(updateStockDto.NomMedicament, StringComparison.OrdinalIgnoreCase))
                {
                    var medicamentResponse = await _mediicamentService.GetByNomAsync(updateStockDto.NomMedicament);
                    if (!medicamentResponse.Success || medicamentResponse.Data == null)
                    {
                        return ApiResponse<StockResponseDto>.ErrorResponse("Médicament non trouvé");
                    }
                    existingStock.MedicamentId = medicamentResponse.Data.Id;
                }

                _mapper.Map(updateStockDto, existingStock);
                await _stockRepository.UpdateAsync(existingStock);

                var result = _mapper.Map<StockResponseDto>(existingStock);
                return ApiResponse<StockResponseDto>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<StockResponseDto>.ErrorResponse(
                    "Erreur lors de la mise à jour du stock",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var stock = await _stockRepository.GetByIdAsync(id);
                if (stock == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Stock non trouvé");
                }
                await _stockRepository.DeleteAsync(id);
                return ApiResponse<bool>.SuccessResponse(true, "Stock supprimé avec succés");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse(
                    "Erreur lors de la suppression du stock",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<StockResponseDto>> GetByMedicamentAndDateRepemptionAsync(int medicamentId, DateTime datePeremption)
        {
            try
            {
                var stock = await _stockRepository.FindByMedicamentAndDatePeremption(medicamentId, datePeremption);
                if(stock == null)
                {
                    return ApiResponse<StockResponseDto>.ErrorResponse("Stock non trouvé");
                }
                var result = _mapper.Map<StockResponseDto>(stock);
                return ApiResponse<StockResponseDto>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<StockResponseDto>.ErrorResponse(
                    "Erreur lors de la récupération du stock",
                    new List<string> { ex.Message });
            }
        }
    }
}
