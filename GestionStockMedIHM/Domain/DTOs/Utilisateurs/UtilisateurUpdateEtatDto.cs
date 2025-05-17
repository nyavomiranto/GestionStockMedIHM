using System.ComponentModel.DataAnnotations;

namespace GestionStockMedIHM.Domain.DTOs.Utilisateurs
{
    public class UtilisateurUpdateEtatDto
    {
        [Required]
        public bool Etat { get; set; }
    }
}
