using System.ComponentModel.DataAnnotations;

namespace GestionStockMedIHM.Domain.DTOs.LigneSortieStocks
{
    public class CreateLigneSortieStockDto
    {
        [Required]
        public int SortieStockId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantite { get; set; }

        [Required]
        public int MedicamentId { get; set; }
    }
}