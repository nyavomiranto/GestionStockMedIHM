using GestionStockMedIHM.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionStockMedIHM.Domain.Entities
{
    public class LigneDemande
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required int Quantite { get; set; }

        public required int MedicamentId { get; set; }
        [ForeignKey("MedicamentId")]
        public Medicament Medicament { get; set; }
        
        public required int DemandeId { get; set; }
        [ForeignKey("DemandeId")]
        public Demande Demande { get; set; }

    }
}
