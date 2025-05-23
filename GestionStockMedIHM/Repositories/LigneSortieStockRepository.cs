using GestionStockMedIHM.Domain.Entities;
using GestionStockMedIHM.Interfaces.LigneDemandes;
using GestionStockMedIHM.Interfaces.LigneSortieStocks;
using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GestionStockMedIHM.Repositories
{
    public class LigneSortieStockRepository : Repository<LigneSortieStock>, ILigneSortieStockRepository
    {
        private readonly AppDbContext _appDbContext;

        public LigneSortieStockRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<LigneSortieStock>> GetAllWithDetailsAsync()
        {
            return await _appDbContext.LigneSortieStocks
                 .Include(l => l.Medicament)
                .ToListAsync();
        }

        public async Task<LigneSortieStock> GetByIdWithDetailsAsync(int id)
        {
            return await _appDbContext.LigneSortieStocks
                .Include(l => l.Medicament)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task AddRangeAsync(List<LigneSortieStock> ligneSortieStocks)
        {
            await _appDbContext.LigneSortieStocks.AddRangeAsync(ligneSortieStocks);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _appDbContext.Database.BeginTransactionAsync();
        }
    }
}
