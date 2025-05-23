using GestionStockMedIHM.Interfaces.SortieStocks;
using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionStockMedIHM.Repositories
{
    public class SortieStockRepository: Repository<SortieStock>, ISortieStockRepository
    {
        private AppDbContext _appDbContext;

        public SortieStockRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<SortieStock>> GetAllWithDetailsAsync()
        {
            return await _appDbContext.SortieStocks
               .Include(d => d.Utilisateur) 
               .Include(d => d.LignesSortieStock) 
                   .ThenInclude(l => l.Medicament) 
               .ToListAsync();
        }

        public async Task<SortieStock> GetByIdWithDetailsAsync(int id)
        {
            return await _appDbContext.SortieStocks
                .Include(d => d.Utilisateur)
                .Include(d => d.LignesSortieStock)
                    .ThenInclude(ld => ld.Medicament)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
