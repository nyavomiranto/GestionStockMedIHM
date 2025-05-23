using GestionStockMedIHM.Domain.DTOs.LignesDemandes;
using GestionStockMedIHM.Domain.DTOs.LigneSortieStocks;

namespace GestionStockMedIHM.Domain.DTOs.SortieStocks
{
    public class SortieStockResponseDto
    {
        public int DemandeId { get; set; }
        public int UtilisateurId { get; set; }
        public DateTime DateSortie { get; set; }
        public List<LigneSortieStockResponseDto> LignesSorties { get; set; }
    }
}
