using System.ComponentModel.DataAnnotations;

namespace GestionStockMedIHM.Domain.DTOs.LignesDemandes
{
    public class CreateLigneDemandeDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantite { get; set; }

        [Required]
        public string NomMedicament { get; set; }
    }
}
