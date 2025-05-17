namespace GestionStockMedIHM.Domain.DTOs.Utilisateurs
{
    public class UtilisateurResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nom { get; set; }
        public string Role { get; set; }
        public bool Etat {  get; set; }
    }
}
