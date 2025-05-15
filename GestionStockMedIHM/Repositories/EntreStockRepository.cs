using GestionStockMedIHM.Interfaces.EntreStocks;
using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionStockMedIHM.Repositories
{
    public class EntreStockRepository: Repository<EntreStock>, IEntreStockRepository
    {
        private AppDbContext _appDbContext;

        public EntreStockRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<EntreStock>> GetAllWithDetailsAsync()
        {
            return await _appDbContext.EntreStocks
                .Include(e => e.Medicament)
                .Include(e => e.Fournisseur)
                .ToListAsync();
        }

        public async Task<EntreStock> GetByIdWithDetailsAsync(int id)
        {
            return await _appDbContext.EntreStocks
                .Include (e => e.Medicament)
                .Include (e => e.Fournisseur)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

    }
}
