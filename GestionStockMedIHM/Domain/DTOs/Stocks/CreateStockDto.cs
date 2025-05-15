using System.ComponentModel.DataAnnotations;

namespace GestionStockMedIHM.Domain.DTOs.Stocks
{
    public class CreateStockDto
    {
        [Required]
        public int Quantite { get; set; }

        [Required]
        [DataType(dataType: DataType.Date)]
        public DateTime DatePeremption { get; set; }

        [Required, StringLength(50)]
        public required string NomMedicament { get; set; }
    }
}
