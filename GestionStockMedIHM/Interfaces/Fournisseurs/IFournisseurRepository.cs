using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Interfaces.Fournisseurs
{
    public interface IFournisseurRepository: IRepository<Fournisseur>
    {
        Task<Fournisseur> GetByNomAsync(string nom);
    }
}
