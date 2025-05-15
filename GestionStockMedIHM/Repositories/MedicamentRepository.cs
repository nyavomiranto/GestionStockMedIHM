using GestionStockMedIHM.Interfaces.Medicaments;
using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionStockMedIHM.Repositories
{
    public class MedicamentRepository : Repository<Medicament>, IMedicamentRepository
    {
        private AppDbContext _appDbContext;
        private readonly ILogger<MedicamentRepository> _logger;

        public MedicamentRepository(AppDbContext appDbContext, ILogger<MedicamentRepository> logger) : base(appDbContext)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task<Medicament> GetByNomAsync(string nom)
        {
            return await _appDbContext.Medicaments
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Nom == nom);
        }
    }
}
