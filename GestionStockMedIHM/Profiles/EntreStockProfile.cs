using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.EntreStocks;
using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Profiles
{
    public class EntreStockProfile: Profile
    {
        public EntreStockProfile() 
        {
            CreateMap<EntreStock, EntreStockResponseDto>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Quantite, opt => opt.MapFrom(src => src.Quantite))
              .ForMember(dest => dest.DateEntre, opt => opt.MapFrom(src => src.DateEntre))
              .ForMember(dest => dest.DatePeremptionMedicament, opt => opt.MapFrom(src => src.DatePeremptionMedicament))
              .ForMember(dest => dest.Motif, opt => opt.MapFrom(src => src.Motif))
              .ForMember(dest => dest.PrixUnitaire, opt => opt.MapFrom(src => src.PrixUnitaire))
              .ForMember(dest => dest.NomFournisseur, opt => opt.MapFrom(src => src.Fournisseur.Nom))
              .ForMember(dest => dest.NomMedicament, opt => opt.MapFrom(src => src.Medicament.Nom));


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
