using GestionStockMedIHM.Domain.DTOs.LignesDemandes;
using GestionStockMedIHM.Domain.Entities;
using GestionStockMedIHM.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionStockMedIHM.Domain.DTOs.Demandes
{
    public class DemandeResponseDto
    {
        public int Id { get; set; }
        public string NomClient {  get; set; }
        public DateTime DateDemande { get; set; }
        public StatutDemande StatutDemande { get; set; }
        public string NomUtilisateur  { get; set; }
        public List<LigneDemandeResponseDto> LignesDemande { get; set; }
    }
}
