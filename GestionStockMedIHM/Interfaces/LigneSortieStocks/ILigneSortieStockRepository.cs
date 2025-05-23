using GestionStockMedIHM.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace GestionStockMedIHM.Interfaces.LigneSortieStocks
{
    public interface ILigneSortieStockRepository: IRepository<LigneSortieStock>
    {
        Task<IEnumerable<LigneSortieStock>> GetAllWithDetailsAsync();
        Task<LigneSortieStock> GetByIdWithDetailsAsync(int id);
        Task AddRangeAsync(List<LigneSortieStock> ligneSortieStocks);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
