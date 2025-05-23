using GestionStockMedIHM.Domain.Entities;
using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Interfaces.LigneDemandes
{
    public interface ILigneDemandeRepository: IRepository<LigneDemande>
    {
        Task<IEnumerable<LigneDemande>> GetAllWithDetailsAsync();
        Task<LigneDemande> GetByIdWithDetailsAsync(int id);
        Task<List<LigneDemande>> GetByDemandeIdAsync(int demandeId);
    }
}
