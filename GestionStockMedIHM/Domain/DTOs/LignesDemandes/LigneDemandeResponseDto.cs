namespace GestionStockMedIHM.Domain.DTOs.LignesDemandes
{
    public class LigneDemandeResponseDto
    {
        public int Id { get; set; }
        public int Quantite { get; set; }
        public int DemandeId { get; set; }

        public int MedicamentId { get; set; }
        public string NomMedicamnet { get; set; }
        public string DosageMedicament { get; set; }
        public string PrixUnitaire { get; set; }
        public decimal? PrixTotal { get; set; }
       
    }
}
