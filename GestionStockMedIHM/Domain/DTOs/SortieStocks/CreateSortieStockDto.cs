using GestionStockMedIHM.Domain.DTOs.LignesDemandes;
using GestionStockMedIHM.Domain.DTOs.LigneSortieStocks;

namespace GestionStockMedIHM.Domain.DTOs.SortieStocks
{
    public class CreateSortieStockDto
    {
        public int DemandeId { get; set; }
        public required int UtilisateurId { get; set; }
        public List<CreateLigneSortieStockDto> LignesSortieStock { get; set; }

    }
}
