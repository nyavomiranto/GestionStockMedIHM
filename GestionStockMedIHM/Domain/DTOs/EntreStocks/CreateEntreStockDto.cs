using System.ComponentModel.DataAnnotations;

namespace GestionStockMedIHM.Domain.DTOs.EntreStocks
{
    public class CreateEntreStockDto
    {
        [Required]
        public int Quantite { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime DatePeremptionMedicament { get; set; }

        [Required, StringLength(100)]
        public string Motif { get; set; }

        [Required]
        public int PrixUnitaire { get; set; }

        [Required, StringLength(50)]
        public string NomMedicament { get; set; }

        [Required, StringLength(50)]
        public string NomFournisseur { get; set; }
    }
}
