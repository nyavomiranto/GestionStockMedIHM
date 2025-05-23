namespace GestionStockMedIHM.Domain.DTOs.Notification
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int DemandeId { get; set; }
        public string Message { get; set; }
        public bool EstVue { get; set; }
        public DateTime DateNotification { get; set; }
        public int UtilisateurId { get; set; }
    }
}
