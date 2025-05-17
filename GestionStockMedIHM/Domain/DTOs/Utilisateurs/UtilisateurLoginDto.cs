using System.ComponentModel.DataAnnotations;

namespace GestionStockMedIHM.Domain.DTOs.Utilisateurs
{
    public class UtilisateurLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string MotDePasse { get; set; }
    }
}
