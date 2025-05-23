using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Stocks;
using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Profiles
{
    public class StockProfile: Profile
    {
        public StockProfile() 
        {
            CreateMap<Stock, StockResponseDto>()
                .ForMember(dest => dest.NomMedicament, opt => opt.MapFrom(src => src.Medicament.Nom))
                .ForMember(dest => dest.DescriptionMedicament, opt => opt.MapFrom(src => src.Medicament.Description))
                .ForMember(dest => dest.FormeMedicament, opt => opt.MapFrom(src => src.Medicament.Forme))
                .ForMember(dest => dest.DosageMedicament, opt => opt.MapFrom(src => src.Medicament.Dosage))
                .ForMember(dest => dest.PrixVenteMedicament, opt => opt.MapFrom(src => src.Medicament.PrixVente));

            CreateMap<CreateStockDto, Stock>()
                .ForMember(dest => dest.MedicamentId, opt => opt.Ignore())
                .ForMember(dest => dest.Medicament, opt => opt.Ignore())
                .ForMember(dest => dest.DatePeremption, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.DatePeremption, DateTimeKind.Utc)));

            CreateMap<UpdateStockDto, Stock>()
                .ForMember(dest => dest.MedicamentId, opt => opt.Ignore())
                .ForMember(dest => dest.Medicament, opt => opt.Ignore())
                 .ForMember(dest => dest.DatePeremption, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.DatePeremption, DateTimeKind.Utc)));
        }   
    }
}
