using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Interfaces.Stocks
{
    public interface IStockRepository: IRepository<Stock>
    {
        Task<IEnumerable<Stock>> GetAllWithDetailsAsync();
        Task<Stock> GetByIdWithDetailsAsync(int id);

        Task<Stock> FindByMedicamentAndDatePeremption(int medicamentId, DateTime datePeremption);
    }
}
