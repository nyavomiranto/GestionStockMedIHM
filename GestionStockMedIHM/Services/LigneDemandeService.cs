using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.EntreStocks;
using GestionStockMedIHM.Domain.DTOs.LignesDemandes;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Domain.Entities;
using GestionStockMedIHM.Interfaces.LigneDemandes;
using GestionStockMedIHM.Interfaces.Medicaments;
using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Services
{
    public class LigneDemandeService : ILigneDemandeService
    {
        private readonly ILigneDemandeRepository _ligneDemandeRepository;
        private readonly IMedicamentService _medicamentService;
        private readonly IMapper _mapper;
        private readonly ILogger<LigneDemandeService> _logger;

        public LigneDemandeService(ILigneDemandeRepository ligneDemandeRepository, IMedicamentService medicamentService, IMapper mapper, ILogger<LigneDemandeService> logger)
        {
            _ligneDemandeRepository = ligneDemandeRepository;
            _medicamentService = medicamentService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<LigneDemandeResponseDto>> CreateAsync(CreateLigneDemandeDto createLigneDemandeDto, int demandeId)
        {
            try
            {
                var medicamentResponse = await _medicamentService.GetByNomAsync(createLigneDemandeDto.NomMedicament);
                if (!medicamentResponse.Success || medicamentResponse.Data == null)
                {
                    return ApiResponse<LigneDemandeResponseDto>.ErrorResponse(
                        "Médicament introuvable",
                        new List<string> { medicamentResponse.Message ?? $"Aucun médicament trouvé avec le nom '{createLigneDemandeDto.NomMedicament}'" });
                }

                var entity = _mapper.Map<LigneDemande>(createLigneDemandeDto);
     

       
                entity.MedicamentId = medicamentResponse.Data.Id;
                entity.DemandeId = demandeId;

                await _ligneDemandeRepository.AddAsync(entity);

                var ligneDemandeWithMedicament = await _ligneDemandeRepository.GetByIdWithDetailsAsync(entity.Id);
                if (ligneDemandeWithMedicament == null)
                {
                    return ApiResponse<LigneDemandeResponseDto>.ErrorResponse(
                        "Erreur lors de la récupération de la ligne de demande après création",
                        new List<string> { "Ligne de demande non trouvée après création" });
                }

                var result = _mapper.Map<LigneDemandeResponseDto>(ligneDemandeWithMedicament);
                return ApiResponse<LigneDemandeResponseDto>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<LigneDemandeResponseDto>.ErrorResponse(
                    "Erreur lors de la création de la ligne de demande",
                    new List<string> { ex.Message });
            }
        }
    }
    }

