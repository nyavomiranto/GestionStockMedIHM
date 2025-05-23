using GestionStockMedIHM.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionStockMedIHM.Models.Entities
{
    public class SortieStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required DateTime DateSortie { get; set; }

        public required int DemandeId { get; set; }
        [ForeignKey("DemandeId")]
        public required Demande Demande { get; set; }

        //Admin qui confirme
        public required int UtilisateurId { get; set; }
        [ForeignKey("UtilisateurId")]
        public required Utilisateur Utilisateur { get; set; }
        public ICollection<LigneSortieStock> LignesSortieStock { get; set; } = new List<LigneSortieStock>();

    }
}
