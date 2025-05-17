using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Interfaces.Utilisateurs
{
    public interface IUtilisateurRepository: IRepository<Utilisateur>
    {
        Task<Utilisateur> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        
    }
}
