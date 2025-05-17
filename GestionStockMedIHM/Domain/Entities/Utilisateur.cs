using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionStockMedIHM.Models.Entities
{
    public class Utilisateur
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Email { get; set; }
        public string Nom { get; set; }
        public byte[] MotDePasseHash { get; set; }
        public byte[] MotDepasseSalt { get; set; }
        public string Role { get; set; }
        public bool Etat { get; set; } = false;
        public ICollection<Demande> Demandes { get; set; } = new List<Demande>();
    }


}
