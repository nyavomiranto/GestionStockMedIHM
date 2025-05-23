using GestionStockMedIHM.Domain.DTOs.LignesDemandes;
using GestionStockMedIHM.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestionStockMedIHM.Domain.DTOs.Demandes
{
    public class CreateDemandeDto
    {
        [Required, StringLength(50)]
        public string NomClient { get; set; }
        public List<CreateLigneDemandeDto> LignesDemande {get; set;}
    }
}
