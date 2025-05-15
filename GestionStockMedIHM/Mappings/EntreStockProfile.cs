using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.EntreStocks;
using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Mappings
{
    public class EntreStockProfile: Profile
    {
        public EntreStockProfile() 
        {
            CreateMap<EntreStock, EntreStockResponseDto>()
                .ForMember(dest => dest.NomMedicament, opt => opt.MapFrom(src => src.Medicament.Nom))
                .ForMember(dest => dest.NomFournisseur, opt => opt.MapFrom(src => src.Fournisseur.Nom));


            CreateMap<CreateEntreStockDto, EntreStock>()
                .ForMember(dest => dest.MedicamentId, opt => opt.Ignore())
                .ForMember(dest => dest.FournisseurId, opt => opt.Ignore())
                .ForMember(dest => dest.Medicament, opt => opt.Ignore())
                .ForMember(dest => dest.Fournisseur, opt => opt.Ignore())
                .ForMember(dest => dest.DateEntre, opt => opt.MapFrom(_ => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)))
                .ForMember(dest => dest.DatePeremptionMedicament,
                opt => opt.MapFrom(src => DateTime.SpecifyKind(
                    src.DatePeremptionMedicament.Date, 
                    DateTimeKind.Utc)));

            CreateMap<UpdateEntreStockDto, EntreStock>()
               .ForMember(dest => dest.MedicamentId, opt => opt.Ignore())
                .ForMember(dest => dest.FournisseurId, opt => opt.Ignore())
                .ForMember(dest => dest.Medicament, opt => opt.Ignore())
                .ForMember(dest => dest.Fournisseur, opt => opt.Ignore())
                .ForMember(dest => dest.DateEntre, opt => opt.MapFrom(_ => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)))
                .ForMember(dest => dest.DatePeremptionMedicament, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.DatePeremptionMedicament.Date, DateTimeKind.Utc)))
                .ForMember(dest => dest.DateEntre, opt => opt.Ignore());
        }
    }
}
