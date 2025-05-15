using System.ComponentModel.DataAnnotations;

namespace GestionStockMedIHM.Domain.DTOs.Medicaments
{
    public class CreateMedicamentDto
    {
        [Required, StringLength(50)]
        public required string Nom { get; set; }

        [Required, StringLength(250)]
        public required string Description { get; set; }

        [Required, StringLength(50)]
        public required string Forme { get; set; }

        [Required, StringLength(25)]
        public required string Dosage { get; set; }

        [Required]
        public required int PrixVente { get; set; }
    }
}
