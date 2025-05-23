using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Demandes;
using GestionStockMedIHM.Domain.DTOs.LignesDemandes;
using GestionStockMedIHM.Domain.DTOs.LigneSortieStocks;
using GestionStockMedIHM.Domain.DTOs.SortieStocks;
using GestionStockMedIHM.Domain.Entities;
using GestionStockMedIHM.Domain.Enums;
using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Profiles
{
    public class SortieStockProfile: Profile
    {
        public SortieStockProfile()
        {
            CreateMap<SortieStock, SortieStockResponseDto>()
                .ForMember(dest => dest.LignesSorties, opt => opt.MapFrom(src => src.LignesSortieStock ?? new List<LigneSortieStock>()));

            CreateMap<CreateSortieStockDto, SortieStock>()
                .ForMember(dest => dest.UtilisateurId, opt => opt.Ignore())
                .ForMember(dest => dest.Utilisateur, opt => opt.Ignore())
                .ForMember(dest => dest.LignesSortieStock, opt => opt.Ignore())
                .ForMember(dest => dest.DateSortie, opt => opt.MapFrom(_ => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)));


            CreateMap<LigneSortieStockResponseDto, LigneSortieStock>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MedicamentId, opt => opt.MapFrom(src => src.MedicamentId))
                .ForMember(dest => dest.SortieStockId, opt => opt.MapFrom(src => src.SortieStockId))
                .ForMember(dest => dest.Quantite, opt => opt.MapFrom(src => src.Quantite))
                .ForMember(dest => dest.Medicament, opt => opt.Ignore())
                .ForMember(dest => dest.SortieStock, opt => opt.Ignore());

            CreateMap<LigneSortieStock, LigneSortieStockResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MedicamentId, opt => opt.MapFrom(src => src.MedicamentId))
                .ForMember(dest => dest.SortieStockId, opt => opt.MapFrom(src => src.SortieStockId))
                .ForMember(dest => dest.Quantite, opt => opt.MapFrom(src => src.Quantite))
                .ForMember(dest => dest.NomMedicamnet, opt => opt.MapFrom(src => src.Medicament != null ? src.Medicament.Nom : null))
                .ForMember(dest => dest.DosageMedicament, opt => opt.MapFrom(src => src.Medicament != null ? src.Medicament.Dosage : null))
                .ForMember(dest => dest.PrixUnitaire, opt => opt.MapFrom(src => src.Medicament != null ? src.Medicament.PrixVente.ToString("F2") : "0"))
                .ForMember(dest => dest.PrixTotal, opt => opt.MapFrom(src => src.Medicament != null ? src.Quantite * src.Medicament.PrixVente : 0m));
        }
    }
}
