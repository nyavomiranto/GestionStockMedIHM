using GestionStockMedIHM.Interfaces.Utilisateurs;
using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionStockMedIHM.Repositories
{
    public class UtilisateurRepository : Repository<Utilisateur>, IUtilisateurRepository
    {
        private readonly AppDbContext _appDbContext;

        public UtilisateurRepository(AppDbContext appDbContext): base(appDbContext) 
        {
            _appDbContext = appDbContext;
        }

        public async Task<Utilisateur> GetByEmailAsync(string email)
        {
            return await _appDbContext.Utilisateurs.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _appDbContext.Utilisateurs.AnyAsync(u => u.Email == email);
        }
    }
}
