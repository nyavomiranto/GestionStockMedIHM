using GestionStockMedIHM.Domain.Entities;
using GestionStockMedIHM.Interfaces.LigneDemandes;
using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionStockMedIHM.Repositories
{
    public class LigneDemandeRepository: Repository<LigneDemande>, ILigneDemandeRepository
    {
        private readonly AppDbContext _appDbContext;

        public LigneDemandeRepository (AppDbContext appDbContext): base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<LigneDemande>> GetAllWithDetailsAsync()
        {
            return await _appDbContext.LigneDemandes
                .Include(l => l.Medicament)
                .ToListAsync();
        }

        public async Task<LigneDemande> GetByIdWithDetailsAsync(int id)
        {
            return await _appDbContext.LigneDemandes
                .Include(l => l.Medicament)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<LigneDemande>> GetByDemandeIdAsync(int demandeId)
        {
            return await _appDbContext.LigneDemandes
                .Include(ld => ld.Medicament)
                .Where(ld => ld.DemandeId == demandeId)
                .ToListAsync();
        }
    }
}
