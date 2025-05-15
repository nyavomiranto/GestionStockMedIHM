namespace GestionStockMedIHM.Domain.DTOs.Stocks
{
    public class StockResponseDto
    {
        public int Id { get; set; }
        public DateTime DatePeremption { get; set; }
        public int Quantite { get; set; }


        public string NomMedicament { get; set; }
        public string DescriptionMedicament { get; set; }
        public string FormeMedicament { get; set; }
        public string DosageMedicament { get; set; }
        public string PrixVenteMedicament { get; set; }

    }
}
