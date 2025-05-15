using GestionStockMedIHM.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionStockMedIHM.Domain.Entities
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required int DemandeId { get; set; }
        [ForeignKey("DemandeId")]
        public required Demande Demande { get; set; }

        public required string Message { get; set; }

        public required bool EstVue { get; set; }

        public required DateTime DateNotification { get; set; }

        public required int UtilisateurId { get; set; } // L’admin concerné
        [ForeignKey("UtilisateurId")]
        public required Utilisateur Utilisateur { get; set; }
    }
}
