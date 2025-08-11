using GestionStockMedIHM.Interfaces.Stocks;
using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace GestionStockMedIHM.Repositories
{
    public class StockRepository : Repository<Stock>, IStockRepository
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

        public async Task<List<Stock>> GetGroupedStocksAsync()
        {
            var groupedStocks = await _appDbContext.Stocks
                .Include(s => s.Medicament)
                .GroupBy(s => new
                {
                    s.MedicamentId,
                    s.Medicament.Nom,
                    s.Medicament.Description,
                    s.Medicament.Forme,
                    s.Medicament.Dosage,
                    s.Medicament.PrixVente
                })
                .Select(g => new Stock
                {
                    MedicamentId = g.Key.MedicamentId,
                    DatePeremption = default(DateTime),
                    Quantite = g.Sum(s => s.Quantite),
                    Medicament = new Medicament
                    {
                        Id = g.Key.MedicamentId,
                        Nom = g.Key.Nom,
                        Description = g.Key.Description,
                        Forme = g.Key.Forme,
                        Dosage = g.Key.Dosage,
                        PrixVente = g.Key.PrixVente
                    }
                })
                .OrderBy(s => s.Medicament.Nom)
                .ToListAsync();

            return groupedStocks;
        }
    }
}
