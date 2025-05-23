using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Demandes;
using GestionStockMedIHM.Domain.DTOs.LignesDemandes;
using GestionStockMedIHM.Domain.DTOs.LigneSortieStocks;
using GestionStockMedIHM.Domain.DTOs.Notification;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Domain.DTOs.SortieStocks;
using GestionStockMedIHM.Domain.Enums;
using GestionStockMedIHM.Interfaces.Demandes;
using GestionStockMedIHM.Interfaces.LigneDemandes;
using GestionStockMedIHM.Interfaces.Notifications;
using GestionStockMedIHM.Interfaces.SortieStocks;
using GestionStockMedIHM.Interfaces.Utilisateurs;
using GestionStockMedIHM.Models.Entities;
using System.Security.Claims;


namespace GestionStockMedIHM.Services
{
    public class DemandeService : IDemandeService
    {
        private readonly IDemandeRepository _demandeRepository;
        private readonly ILigneDemandeRepository _ligneDemandeRepository;
        private readonly ILigneDemandeService _ligneDemandeService;
        private readonly ISortieStockService _sortieStockService;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUtilisateurRepository _utilisateurRepository;
        private readonly IMapper _mapper;

        public DemandeService(IDemandeRepository demandeRepository, ILigneDemandeRepository ligneDemandeRepository, ILigneDemandeService ligneDemandeService, ISortieStockService sortieStockService, IMapper mapper, INotificationService notificationService, IHttpContextAccessor httpContextAccessor, IUtilisateurRepository utilisateurRepository)
        {
            _demandeRepository = demandeRepository;
            _ligneDemandeRepository = ligneDemandeRepository;
            _ligneDemandeService = ligneDemandeService;
            _sortieStockService = sortieStockService;
            _mapper = mapper;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _utilisateurRepository = utilisateurRepository;
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

                
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId) || userId == 0)
                {
                    return ApiResponse<DemandeResponseDto>.ErrorResponse("Utilisateur non authentifié ou ID invalide");
                }

                demande.UtilisateurId = userId;

                await _demandeRepository.AddAsync(demande);

                foreach (var ligneDto in createDemandeDto.LignesDemande ?? new List<CreateLigneDemandeDto>())
                {
                    var ligneResponse = await _ligneDemandeService.CreateAsync(ligneDto, demande.Id);
                    if (!ligneResponse.Success)
                    {
                        return ApiResponse<DemandeResponseDto>.ErrorResponse(
                            "Échec lors de la création d'une ligne de demande",
                            new List<string> { ligneResponse.Message });
                    }
                }

                var demandeWithRelations = await _demandeRepository.GetByIdWithDetailsAsync(demande.Id);
                if (demandeWithRelations == null)
                {
                    return ApiResponse<DemandeResponseDto>.ErrorResponse(
                        "Erreur lors de la récupération de la demande après création",
                        new List<string> { "Demande non trouvée après création" });
                }

                var adminId = await _utilisateurRepository.GetAdminIdAsync();
                var notificationDto = new NotificationDto
                {
                    DemandeId = demande.Id,
                    Message = $"Nouvelle demande #{demande.Id} créée par l'utilisateur {userId} le {DateTime.UtcNow}",
                    EstVue = false
                };
                await _notificationService.SendNotificationAsync(adminId, notificationDto);

                var result = _mapper.Map<DemandeResponseDto>(demandeWithRelations);
                return ApiResponse<DemandeResponseDto>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<DemandeResponseDto>.ErrorResponse(
                    "Erreur lors de la création de la demande",
                    new List<string> { ex.Message });
            }
        }
        public async Task<ApiResponse<string>> ConfirmDemandeAsync(int demandeId)
        {
            try
            {
                var demande = await _demandeRepository.GetByIdAsync(demandeId);
                if (demande == null)
                {
                    return ApiResponse<string>.ErrorResponse("Demande non trouvée");
                }

                var adminId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (adminId == 0 || _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "Admin")
                {
                    return ApiResponse<string>.ErrorResponse("Accès refusé");
                }

                demande.StatutDemande = StatutDemande.Acceptee;
                await _demandeRepository.UpdateAsync(demande);

                var lignesDemande = await _ligneDemandeRepository.GetByDemandeIdAsync(demandeId);
                if (lignesDemande == null)
                {
                    return ApiResponse<string>.ErrorResponse("Erreur lors de la récupération des lignes de demande");
                }

                if (!lignesDemande.Any())
                {
                    return ApiResponse<string>.ErrorResponse("Aucune ligne de demande trouvée pour cette demande");
                }

                var ligneSortieStockDtos = _mapper.Map<List<CreateLigneSortieStockDto>>(lignesDemande);

                var createSortieStockDto = new CreateSortieStockDto
                {
                    DemandeId = demandeId,
                    UtilisateurId = adminId,
                    LignesSortieStock = ligneSortieStockDtos
                };

                var sortieStockResult = await _sortieStockService.CreateSortieStockAsync(createSortieStockDto);
                if (!sortieStockResult.Success)
                {
                    return ApiResponse<string>.ErrorResponse(
                        "Erreur lors de la création de la sortie de stock",
                        sortieStockResult.Errors);
                }

                var notificationDto = new NotificationDto
                {
                    DemandeId = demande.Id,
                    Message = $"Votre demande #{demande.Id} a été confirmée par l'administrateur le {DateTime.UtcNow}.",
                    EstVue = false
                };
                await _notificationService.SendNotificationAsync(demande.UtilisateurId, notificationDto);

                return ApiResponse<string>.SuccessResponse("Demande confirmée et sortie de stock créée avec succès");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.ErrorResponse(
                    "Erreur lors de la confirmation de la demande",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<string>> RefuseDemandeAsync(int demandeId)
        {
            try
            {
                var demande = await _demandeRepository.GetByIdAsync(demandeId);
                if (demande == null)
                {
                    return ApiResponse<string>.ErrorResponse("Demande non trouvée");
                }

                var adminId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (adminId == 0 || _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "Admin")
                {
                    return ApiResponse<string>.ErrorResponse("Accès refusé");
                }

                demande.StatutDemande = StatutDemande.Annulee;
                await _demandeRepository.UpdateAsync(demande);

                var notificationDto = new NotificationDto
                {
                    DemandeId = demande.Id,
                    Message = $"Votre demande #{demande.Id} a été refusée par l'administrateur le {DateTime.UtcNow}.",
                    EstVue = false
                };
                await _notificationService.SendNotificationAsync(demande.UtilisateurId, notificationDto);

                return ApiResponse<string>.SuccessResponse("Demande refusée");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.ErrorResponse(
                   "Erreur lors du refus de la demande",
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
