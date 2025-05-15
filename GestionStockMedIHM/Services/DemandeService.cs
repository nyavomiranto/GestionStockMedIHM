using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Demandes;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Domain.Entities;
using GestionStockMedIHM.Interfaces.Demandes;
using GestionStockMedIHM.Interfaces.LigneDemandes;
using GestionStockMedIHM.Models.Entities;


namespace GestionStockMedIHM.Services
{
    public class DemandeService : IDemandeService
    {
        private readonly IDemandeRepository _demandeRepository;
        private readonly ILigneDemandeService _ligneDemandeService;
        private readonly IMapper _mapper;

        public DemandeService(IDemandeRepository demandeRepository, ILigneDemandeService ligneDemandeService, IMapper mapper)
        {
            _demandeRepository = demandeRepository;
            _ligneDemandeService = ligneDemandeService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<DemandeResponseDto>> GetByIdAsync(int id)
        {
            try
            {
                var demande = await _demandeRepository.GetByIdWithDetailsAsync(id);
                if (demande == null)
                {
                    return ApiResponse<DemandeResponseDto>.ErrorResponse("Demande non trouvé");
                }
                var result = _mapper.Map<DemandeResponseDto>(demande);
                return ApiResponse<DemandeResponseDto>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<DemandeResponseDto>.ErrorResponse(
                    "Erreur lors de la recupération du demande",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<IEnumerable<DemandeResponseDto>>> GetAllAsync()
        {
            try
            {
                var demandes = await _demandeRepository.GetAllWithDetailsAsync();
                var result = _mapper.Map<IEnumerable<DemandeResponseDto>>(demandes);
                return ApiResponse<IEnumerable<DemandeResponseDto>>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<DemandeResponseDto>>.ErrorResponse(
                   "Erreur lors de la recupération des demandes",
                   new List<string> { ex.Message });
            }

        }

        public async Task<ApiResponse<DemandeResponseDto>> CreateAsync(CreateDemandeDto createDemandeDto)
        {
            try
            {

                var demande = _mapper.Map<Demande>(createDemandeDto);
                demande.UtilisateurId = 1; // REMPLACER PAR LE ID DANS LE TOKEN

                await _demandeRepository.AddAsync(demande);

                foreach (var ligneDto in createDemandeDto.LignesDemande)
                {
                    var ligneResponse = await _ligneDemandeService.CreateAsync(ligneDto, demande.Id);
                    if (!ligneResponse.Success)
                    {
                        return ApiResponse<DemandeResponseDto>.ErrorResponse(
                            "Échec lors de la création d'une ligne de demande",
                            new List<string> { ligneResponse.Message });
                    }
                }

                // Recharger la Demande avec ses relations via le repository
                var demandeWithRelations = await _demandeRepository.GetByIdWithDetailsAsync(demande.Id);
                if (demandeWithRelations == null)
                {
                    return ApiResponse<DemandeResponseDto>.ErrorResponse(
                        "Erreur lors de la récupération de la demande après création",
                        new List<string> { "Demande non trouvée après création" });
                }

                var result = _mapper.Map<DemandeResponseDto>(demandeWithRelations);
                return ApiResponse<DemandeResponseDto>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<DemandeResponseDto>.ErrorResponse(
                       "Erreur lors de la creation du demande",
                       new List<string> { ex.Message });
            }
        }
        public async Task<ApiResponse<DemandeResponseDto>> UpdateAsync(int id, UpdateDemandeDto updateDemandeDto)
        {
            try
            {
                var existingDemande = await _demandeRepository.GetByIdAsync(id);
                if (existingDemande == null)
                {
                    return ApiResponse<DemandeResponseDto>.ErrorResponse("Demande introuvable");
                }

                _mapper.Map(updateDemandeDto, existingDemande);
                await _demandeRepository.UpdateAsync(existingDemande);

                var result = _mapper.Map<DemandeResponseDto>(existingDemande);
                return ApiResponse<DemandeResponseDto>.SuccessResponse(result, "Demande mise à jour avec succès");
            }
            catch (Exception ex)
            {
                return ApiResponse<DemandeResponseDto>.ErrorResponse(
                       "Erreur lors de la mise a jour du demande",
                       new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var existingDemande = await _demandeRepository.GetByIdAsync(id);
                if (existingDemande == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Demande non trouvé");
                }

                await _demandeRepository.DeleteAsync(id);
                return ApiResponse<bool>.SuccessResponse(true, "Demande supprimé avec succes");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse(
                       "Erreur lors de la suppression du demande",
                       new List<string> { ex.Message });

            }
        }
    }
}
