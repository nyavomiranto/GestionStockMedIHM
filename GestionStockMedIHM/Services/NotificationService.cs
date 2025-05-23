using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Notification;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Domain.DTOs.Utilisateurs;
using GestionStockMedIHM.Domain.Entities;
using GestionStockMedIHM.Hubs;
using GestionStockMedIHM.Interfaces.Notifications;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace GestionStockMedIHM.Services
{
    public class NotificationService: INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationService(INotificationRepository notificationRepository, IMapper mapper, IHubContext<NotificationHub> hubContext, IHttpContextAccessor httpContextAccessor)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendNotificationAsync(int userId, NotificationDto notificationDto)
        {
            var notification = _mapper.Map<Notification>(notificationDto);
            notification.UtilisateurId = userId;
            notification.DateNotification = DateTime.UtcNow;
            await _notificationRepository.AddAsync(notification);

            // Mettre à jour l'ID dans le DTO après persistance
            notificationDto.Id = notification.Id;
            notificationDto.DateNotification = notification.DateNotification;
            notificationDto.UtilisateurId = userId;

            // Envoyer la notification via SignalR
            await _hubContext.Clients.User(userId.ToString())
                .SendAsync("ReceiveNotification", notificationDto);
        }

        public async Task<ApiResponse<IEnumerable<NotificationDto>>> GetAllAsync()
        {
            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId) || userId == 0)
                {
                    return ApiResponse<IEnumerable<NotificationDto>>.ErrorResponse("Utilisateur non authentifié ou ID invalide");
                }

                var notifications = await _notificationRepository.GetAllUserById(userId);
                var result = _mapper.Map<IEnumerable<NotificationDto>>(notifications);
                return ApiResponse<IEnumerable<NotificationDto>>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<NotificationDto>>.ErrorResponse(
                    "Erreur lors de la récupération des notifications",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var notification = _notificationRepository.GetByIdAsync(id);
                if (notification == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Notification non trouvé");
                }
                await _notificationRepository.DeleteAsync(id);
                return ApiResponse<bool>.SuccessResponse(true, "Notification supprimé avec succés");
            }
            catch (Exception ex) 
            { 
                return ApiResponse<bool>.ErrorResponse(
                    "Erreur lors de la suppression du notification", 
                    new List<string> { ex.Message });   
            }
        }
    }
}
