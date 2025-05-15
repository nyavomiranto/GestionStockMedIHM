using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Interfaces.EntreStocks
{
    public interface IEntreStockRepository: IRepository<EntreStock>
    {
        Task<IEnumerable<EntreStock>> GetAllWithDetailsAsync();
        Task<EntreStock> GetByIdWithDetailsAsync(int id);
    }
}
