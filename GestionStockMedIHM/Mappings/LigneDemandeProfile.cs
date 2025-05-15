using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.LignesDemandes;
using GestionStockMedIHM.Domain.Entities;

namespace GestionStockMedIHM.Mappings
{
    public class LigneDemandeProfile : Profile
    {
        public LigneDemandeProfile()
        {
            CreateMap<CreateLigneDemandeDto, LigneDemande>()
                .ForMember(dest => dest.MedicamentId, opt => opt.Ignore())
                .ForMember(dest => dest.DemandeId, opt => opt.Ignore())
                .ForMember(dest => dest.Medicament, opt => opt.Ignore())
                .ForMember(dest => dest.Demande, opt => opt.Ignore());
        }
    }
}