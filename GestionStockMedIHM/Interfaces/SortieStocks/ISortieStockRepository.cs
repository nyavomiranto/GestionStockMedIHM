using GestionStockMedIHM.Domain.Entities;
using GestionStockMedIHM.Models.Entities;
using GestionStockMedIHM.Repositories;

namespace GestionStockMedIHM.Interfaces.SortieStocks
{
    public interface ISortieStockRepository: IRepository<SortieStock>
    {
        Task<IEnumerable<SortieStock>> GetAllWithDetailsAsync();
        Task<SortieStock> GetByIdWithDetailsAsync(int id);
    }
}
