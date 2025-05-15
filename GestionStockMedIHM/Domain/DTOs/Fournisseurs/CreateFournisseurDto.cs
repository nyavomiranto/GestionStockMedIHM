using System.ComponentModel.DataAnnotations;

namespace GestionStockMedIHM.Domain.DTOs.Fournisseurs
{
    public class CreateFournisseurDto
    {
        [Required, StringLength(50)]
        public required string Nom { get; set; }

        [Required, StringLength(50)]
        public required string Contact { get; set; }

        [Required, StringLength(50)]
        public required string Adresse { get; set; }
    }
}
