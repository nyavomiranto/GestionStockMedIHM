using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionStockMedIHM.Models.Entities
{
    public class Stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required DateTime DatePeremption { get; set; }

        public required int Quantite { get; set; }

        public required int MedicamentId {  get; set; }
        [ForeignKey("MedicamentId")]
        public required Medicament Medicament { get; set; }

    }
}
