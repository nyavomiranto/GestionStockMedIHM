namespace GestionStockMedIHM.Domain.DTOs.Medicaments
{
    public class MedicamentDto
    {

        public int Id { get; set; }

        public string Nom { get; set; }

        public  string Description { get; set; }

        public string Forme { get; set; }

        public string Dosage { get; set; }

        public int PrixVente { get; set; }
    }
}
