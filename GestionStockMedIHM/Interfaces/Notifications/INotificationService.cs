using GestionStockMedIHM.Domain.DTOs.Notification;
using GestionStockMedIHM.Domain.DTOs.Responses;

namespace GestionStockMedIHM.Interfaces.Notifications
{
    public interface INotificationService
    {
        Task SendNotificationAsync(int userId, NotificationDto notification);
        Task<ApiResponse<IEnumerable<NotificationDto>>> GetAllAsync();
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
