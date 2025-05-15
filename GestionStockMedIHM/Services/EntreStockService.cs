using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.EntreStocks;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Domain.DTOs.Stocks;
using GestionStockMedIHM.Interfaces.EntreStocks;
using GestionStockMedIHM.Interfaces.Fournisseurs;
using GestionStockMedIHM.Interfaces.Medicaments;
using GestionStockMedIHM.Interfaces.Stocks;
using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Services
{
    public class EntreStockService : IEntreStockService
    {
        private readonly IEntreStockRepository _entreStockRepository;
        private readonly IMedicamentService _mediicamentService;
        private readonly IFournisseurService _fournisseurService;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        public EntreStockService(IEntreStockRepository entreStockRepository, IMapper mapper, IMedicamentService mediicamentService, IFournisseurService fournisseurService, IStockService stockService)
        {
            _entreStockRepository = entreStockRepository;
            _mediicamentService = mediicamentService;
            _fournisseurService = fournisseurService;
            _stockService = stockService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<EntreStockResponseDto>> GetByIdAsync(int id)
        {
            try
            {
                var entreStock = await _entreStockRepository.GetByIdWithDetailsAsync(id);
                if (entreStock == null)
                {
                    return ApiResponse<EntreStockResponseDto>.ErrorResponse("Entrée de stock non trouvé");
                }
                var result = _mapper.Map<EntreStockResponseDto>(entreStock);
                return ApiResponse<EntreStockResponseDto>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<EntreStockResponseDto>.ErrorResponse(
                    "Erreur lors de la récupération de l'entrée du stock",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<IEnumerable<EntreStockResponseDto>>> GetAllAsync()
        {
            try
            {
                var entreStock = await _entreStockRepository.GetAllWithDetailsAsync();
                var result = _mapper.Map<IEnumerable<EntreStockResponseDto>>(entreStock);
                return ApiResponse<IEnumerable<EntreStockResponseDto>>.SuccessResponse(result);
            }
            catch (Exception ex) 
            {
                return ApiResponse<IEnumerable<EntreStockResponseDto>>.ErrorResponse(
                    "Erreur lors de la récupération des entrées de stocks",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<EntreStockResponseDto>> CreateAsync(CreateEntreStockDto createEntreStockDto)
        {
            try
            {
                var medicamentResponse = await _mediicamentService.GetByNomAsync(createEntreStockDto.NomMedicament);
                if (!medicamentResponse.Success || medicamentResponse.Data == null)
                {
                    return ApiResponse<EntreStockResponseDto>.ErrorResponse("Médicament non trouvé");
                }

                var fournisseurResponse = await _fournisseurService.GetByNomAsync(createEntreStockDto.NomFournisseur);
                if (!fournisseurResponse.Success || fournisseurResponse.Data == null)
                {
                    return ApiResponse<EntreStockResponseDto>.ErrorResponse("Fournisseur non trouvé");
                }

                var medicamentId = medicamentResponse.Data.Id;
                var datePeremption = DateTime.SpecifyKind(createEntreStockDto.DatePeremptionMedicament.Date, DateTimeKind.Utc); //conversion avant comparaison

                var stockResponse =  await _stockService.GetByMedicamentAndDateRepemptionAsync(medicamentId, datePeremption);

                if (stockResponse.Success && stockResponse.Data != null)
                {
                    var updateStockDto = new UpdateStockDto
                    {
                        Quantite = stockResponse.Data.Quantite + createEntreStockDto.Quantite,
                        NomMedicament = createEntreStockDto.NomMedicament,
                        DatePeremption = datePeremption,
                    };

                    await _stockService.UpdateAsync(stockResponse.Data.Id, updateStockDto);

                }
                else
                {
                    var createStockDto = new CreateStockDto
                    {
                        NomMedicament = createEntreStockDto.NomMedicament,
                        DatePeremption = datePeremption,
                        Quantite = createEntreStockDto.Quantite
                    };

                    await _stockService.CreateAsync(createStockDto);
                }

                var entity = _mapper.Map<EntreStock>(createEntreStockDto);
                entity.MedicamentId = medicamentResponse.Data.Id;
                entity.FournisseurId = fournisseurResponse.Data.Id;

                await _entreStockRepository.AddAsync(entity);

                var resultDto = _mapper.Map<EntreStockResponseDto>(entity);
                return ApiResponse<EntreStockResponseDto>.SuccessResponse(resultDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<EntreStockResponseDto>.ErrorResponse(
                    "Erreur lors de la creation de l'entrée de stock",
                    new List<string> { ex.Message });

            }

        }

        public async Task<ApiResponse<EntreStockResponseDto>> UpdateAsync(int id, UpdateEntreStockDto updateEntreStockDto)
        {
            try
            {
                var existingEntreStock = await _entreStockRepository.GetByIdWithDetailsAsync(id);
                if (existingEntreStock == null)
                {
                    return ApiResponse<EntreStockResponseDto>.ErrorResponse("Entrée de stock non trouvé");
                }

                //verification si le medicament a changé
                if (!string.IsNullOrWhiteSpace(updateEntreStockDto.NomMedicament) && !existingEntreStock.Medicament.Nom.Equals(updateEntreStockDto.NomMedicament, StringComparison.OrdinalIgnoreCase))
                {
                    var medicamentResponse = await _mediicamentService.GetByNomAsync(updateEntreStockDto.NomMedicament);
                    if (!medicamentResponse.Success || medicamentResponse.Data == null)
                    {
                        return ApiResponse<EntreStockResponseDto>.ErrorResponse("Médicament non trouvé");
                    }
                    existingEntreStock.MedicamentId = medicamentResponse.Data.Id;
                }

                //Verification si le fournisseur n'a pas changé
                if (!string.IsNullOrWhiteSpace(updateEntreStockDto.NomFournisseur) && !existingEntreStock.Fournisseur.Nom.Equals(updateEntreStockDto.NomFournisseur, StringComparison.OrdinalIgnoreCase))
                {
                    var fournisseurResponse = await _fournisseurService.GetByNomAsync(updateEntreStockDto.NomFournisseur);
                    if (!fournisseurResponse.Success || fournisseurResponse.Data == null)
                    {
                        return ApiResponse<EntreStockResponseDto>.ErrorResponse("Fournisseur non trouvé");
                    }
                    existingEntreStock.FournisseurId = fournisseurResponse.Data.Id;
                }

                _mapper.Map(updateEntreStockDto, existingEntreStock);
                await _entreStockRepository.UpdateAsync(existingEntreStock);

                var result = _mapper.Map<EntreStockResponseDto>(existingEntreStock);
                return ApiResponse<EntreStockResponseDto>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<EntreStockResponseDto>.ErrorResponse(
                    "Erreur lors de la mise à jour de l'entrée de stock",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var entreStock = await _entreStockRepository.GetByIdAsync(id);
                if (entreStock == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Entrée de Stock non trouvé");
                }
                await _entreStockRepository.DeleteAsync(id);
                return ApiResponse<bool>.SuccessResponse(true, "Entrée de stock supprimé avec succés");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse(
                        "Erreur lors de la suppression de l'entrée de stock",
                        new List<string> { ex.Message });
            }
        }
    }

}

