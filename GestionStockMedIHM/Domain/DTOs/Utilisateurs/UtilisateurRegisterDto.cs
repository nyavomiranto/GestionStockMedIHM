using System.ComponentModel.DataAnnotations;

namespace GestionStockMedIHM.Domain.DTOs.Utilisateurs
{
    public class UtilisateurRegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Nom { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
        public string MotDePasse { get; set; }
        
        [Required]
        public string Role { get; set; }
    }
}
