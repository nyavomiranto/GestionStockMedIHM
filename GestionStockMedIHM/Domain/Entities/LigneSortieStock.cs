using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionStockMedIHM.Domain.Entities
{
    public class LigneSortieStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set; }
        public required int Quantite { get; set; }
        public required int MedicamentId { get; set; }
        [ForeignKey("MedicamentId")]
        public required Medicament Medicament { get; set; }
        public int SortieStockId { get; set; }
        [ForeignKey("SortieStockId")]
        public required SortieStock SortieStock { get; set; }

    }
}
