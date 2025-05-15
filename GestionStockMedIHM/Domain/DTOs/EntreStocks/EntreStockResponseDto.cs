using GestionStockMedIHM.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionStockMedIHM.Domain.DTOs.EntreStocks
{
    public class EntreStockResponseDto
    {
        public int Id { get; set; }

        public int Quantite { get; set; }

        public DateTime DateEntre { get; set; }

        public  DateTime DatePeremptionMedicament { get; set; }

        public string Motif { get; set; }

        public int PrixUnitaire { get; set; }

        public  string NomFournisseur { get; set; }

        public string NomMedicament { get; set; }
    }
}
