using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GestionStockMedIHM.Domain.Entities;
using GestionStockMedIHM.Domain.Enums;

namespace GestionStockMedIHM.Models.Entities
{
    public class Demande
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string NomClient { get; set; }

        public required DateTime DateDemande { get; set; }

        public StatutDemande StatutDemande { get; set; }

        public int UtilisateurId { get; set; }
        [ForeignKey("UtilisateurId")]
        public Utilisateur Utilisateur { get; set; }

        public ICollection<LigneDemande> LignesDemande { get; set; } = new List<LigneDemande>();

    }
}
