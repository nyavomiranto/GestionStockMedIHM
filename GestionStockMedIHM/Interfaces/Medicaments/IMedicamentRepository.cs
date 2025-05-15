using GestionStockMedIHM.Models.Entities;
using GestionStockMedIHM.Repositories;

namespace GestionStockMedIHM.Interfaces.Medicaments
{
    public interface IMedicamentRepository: IRepository<Medicament>
    {
        Task<Medicament> GetByNomAsync(string nom);
    }
}
