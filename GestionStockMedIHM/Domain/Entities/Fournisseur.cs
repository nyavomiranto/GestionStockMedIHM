using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionStockMedIHM.Models.Entities
{
    public class Fournisseur
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Nom { get; set; }

        public required string Contact {  get; set; }

        public required string Adresse { get; set; }
    }
}
