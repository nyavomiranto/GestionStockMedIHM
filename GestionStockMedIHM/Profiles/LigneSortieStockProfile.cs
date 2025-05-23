using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.LignesDemandes;
using GestionStockMedIHM.Domain.DTOs.LigneSortieStocks;
using GestionStockMedIHM.Domain.Entities;

namespace GestionStockMedIHM.Profiles
{
    public class LigneSortieStockProfile: Profile
    {
        public LigneSortieStockProfile()
        {
            CreateMap<CreateLigneSortieStockDto, LigneSortieStock>()
                .ForMember(dest => dest.Quantite, opt => opt.MapFrom(src => src.Quantite))
                .ForMember(dest => dest.MedicamentId, opt => opt.MapFrom(src => src.MedicamentId))
                .ForMember(dest => dest.SortieStockId, opt => opt.MapFrom(src => src.SortieStockId))
                .ForMember(dest => dest.Medicament, opt => opt.Ignore())
                .ForMember(dest => dest.SortieStock, opt => opt.Ignore());

            CreateMap<LigneDemande, CreateLigneSortieStockDto>()
                .ForMember(dest => dest.Quantite, opt => opt.MapFrom(src => src.Quantite))
                .ForMember(dest => dest.MedicamentId, opt => opt.MapFrom(src => src.MedicamentId))
                .ForMember(dest => dest.SortieStockId, opt => opt.Ignore());
        }
    }
}
