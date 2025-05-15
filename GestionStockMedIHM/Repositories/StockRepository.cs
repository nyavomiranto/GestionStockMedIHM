using GestionStockMedIHM.Interfaces.Stocks;
using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionStockMedIHM.Repositories
{
    public class StockRepository: Repository<Stock>, IStockRepository
    {
        private AppDbContext _appDbContext;

        public StockRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Stock>> GetAllWithDetailsAsync()
        {
            return await _appDbContext.Stocks.Include(e => e.Medicament).ToListAsync();
        }

        public async Task<Stock> GetByIdWithDetailsAsync(int id)
        {
            return await _appDbContext.Stocks.Include(e => e.Medicament).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Stock> FindByMedicamentAndDatePeremption(int medicamentId, DateTime datePeremption) 
        {
            return await _appDbContext.Stocks.FirstOrDefaultAsync(s => s.MedicamentId == medicamentId && s.DatePeremption.Date == datePeremption.Date);
        }
    }
}
