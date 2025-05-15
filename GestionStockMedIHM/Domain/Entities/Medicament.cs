using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionStockMedIHM.Models.Entities
{
    public class Medicament
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Nom {  get; set; }

        public required string Description { get; set; }

        public required string Forme { get; set; }

        public required string Dosage {  get; set; } 

        public required int PrixVente { get; set; }
    }
}
