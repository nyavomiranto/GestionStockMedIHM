using GestionStockMedIHM.Interfaces.Fournisseurs;
using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionStockMedIHM.Repositories
{
    public class FournisseurRepository : Repository<Fournisseur>, IFournisseurRepository
    {
        private AppDbContext _appDbContext;

        public FournisseurRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Fournisseur> GetByNomAsync(string nom)
        {
            return await _appDbContext.Fournisseurs.FirstOrDefaultAsync(m => m.Nom == nom);
        }

    }
}
