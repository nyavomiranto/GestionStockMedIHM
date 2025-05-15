using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionStockMedIHM.Models.Entities
{
    public class EntreStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required int Quantite { get; set; }
        
        public required DateTime DateEntre { get; set; }
        
        public required DateTime DatePeremptionMedicament { get; set; }

        public required string Motif {  get; set; }

        public required int PrixUnitaire { get; set; }

        public required int FournisseurId { get; set; }
        [ForeignKey("FournisseurId")]
        public required Fournisseur Fournisseur { get; set; }

        public required int MedicamentId { get; set; }
        [ForeignKey("MedicamentId")]
        public required Medicament Medicament { get; set; }
    }
}
