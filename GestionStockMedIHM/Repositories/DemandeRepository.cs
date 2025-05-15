using GestionStockMedIHM.Domain.Entities;
using GestionStockMedIHM.Interfaces;
using GestionStockMedIHM.Interfaces.Demandes;
using GestionStockMedIHM.Interfaces.EntreStocks;
using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionStockMedIHM.Repositories
{
    public class DemandeRepository : Repository<Demande>, IDemandeRepository
    {
        private AppDbContext _appDbContext;

        public DemandeRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Demande>> GetAllWithDetailsAsync()
        {
            return await _appDbContext.Demandes
               .Include(d => d.Utilisateur) // Charge l'utilisateur associé
               .Include(d => d.LignesDemande) // Charge les lignes de demande
                   .ThenInclude(l => l.Medicament) // Charge le médicament de chaque ligne
               .ToListAsync();
        }

        public async Task<Demande> GetByIdWithDetailsAsync(int id)
        {
            return await _appDbContext.Demandes
                .Include(d => d.LignesDemande)
                    .ThenInclude(ld => ld.Medicament)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
