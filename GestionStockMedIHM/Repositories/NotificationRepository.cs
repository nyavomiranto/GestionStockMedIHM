using GestionStockMedIHM.Domain.Entities;
using GestionStockMedIHM.Interfaces.Notifications;
using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionStockMedIHM.Repositories
{
    public class NotificationRepository: Repository<Notification>, INotificationRepository
    {
        private readonly AppDbContext _appDbContext;

        public NotificationRepository(AppDbContext appDbContext): base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Notification>> GetAllUserById(int userId)
        {
            return await _appDbContext.Notifications.Where(n => n.UtilisateurId == userId).ToListAsync();
        }

    }
}
