using GestionStockMedIHM.Interfaces.Stocks;
using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;

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
            var stock = await _appDbContext.Stocks
                .Include(s => s.Medicament)
                .FirstOrDefaultAsync(s => s.Id == id);
            return stock;
        }

        public async Task<Stock> FindByMedicamentAndDatePeremption(int medicamentId, DateTime datePeremption)
        {
            var normaliseDate = datePeremption.Date;
            return await _appDbContext.Stocks
                .FirstOrDefaultAsync(s => s.MedicamentId == medicamentId
                    && s.DatePeremption.Date == normaliseDate);
        }

        public async Task<List<Stock>> GetStocksByMedicamentIdAsync(int medicamentId)
        {
            return await _appDbContext.Stocks
                .Where(s => s.MedicamentId == medicamentId && s.Quantite > 0)
                .OrderBy(s => s.DatePeremption)
                .ToListAsync();
        }
    }
}
