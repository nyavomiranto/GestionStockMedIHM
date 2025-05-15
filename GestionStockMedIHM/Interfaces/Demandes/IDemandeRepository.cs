using GestionStockMedIHM.Domain.Entities;
using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Interfaces.Demandes
{
    public interface IDemandeRepository : IRepository<Demande>
    {
        Task<Demande> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Demande>> GetAllWithDetailsAsync();
    }
}
