using GestionStockMedIHM.Domain.Entities;

namespace GestionStockMedIHM.Interfaces.Notifications
{
    public interface INotificationRepository:IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetAllUserById(int userId);
    }
}
