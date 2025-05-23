using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Demandes;
using GestionStockMedIHM.Domain.DTOs.LignesDemandes;
using GestionStockMedIHM.Domain.Entities;
using GestionStockMedIHM.Domain.Enums;
using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Profiles
{
    public class DemandeProfile : Profile
    {
        public DemandeProfile()
        {
            CreateMap<Demande, DemandeResponseDto>()
                .ForMember(dest => dest.NomUtilisateur, opt => opt.MapFrom(src => src.Utilisateur != null ? src.Utilisateur.Nom : null))
                .ForMember(dest => dest.LignesDemande, opt => opt.MapFrom(src => src.LignesDemande ?? new List<LigneDemande>()))
                .ForMember(dest => dest.StatutDemande, opt => opt.MapFrom(src => src.StatutDemande.ToString()));

            CreateMap<CreateDemandeDto, Demande>()
                .ForMember(dest => dest.UtilisateurId, opt => opt.Ignore())
                .ForMember(dest => dest.Utilisateur, opt => opt.Ignore())
                .ForMember(dest => dest.LignesDemande, opt => opt.Ignore())
                .ForMember(dest => dest.DateDemande, opt => opt.MapFrom(_ => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)))
                .ForMember(dest => dest.StatutDemande, opt => opt.MapFrom(_ => StatutDemande.EnAttente));

            CreateMap<UpdateDemandeDto, Demande>()
                .ForMember(dest => dest.UtilisateurId, opt => opt.Ignore())
                .ForMember(dest => dest.Utilisateur, opt => opt.Ignore())
                .ForMember(dest => dest.DateDemande, opt => opt.MapFrom(_ => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)))
                .ForMember(dest => dest.DateDemande, opt => opt.Ignore());

            CreateMap<LigneDemandeResponseDto, LigneDemande>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MedicamentId, opt => opt.MapFrom(src => src.MedicamentId))
                .ForMember(dest => dest.DemandeId, opt => opt.MapFrom(src => src.DemandeId))
                .ForMember(dest => dest.Quantite, opt => opt.MapFrom(src => src.Quantite))
                .ForMember(dest => dest.Medicament, opt => opt.Ignore())
                .ForMember(dest => dest.Demande, opt => opt.Ignore());

            CreateMap<LigneDemande, LigneDemandeResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MedicamentId, opt => opt.MapFrom(src => src.MedicamentId))
                .ForMember(dest => dest.DemandeId, opt => opt.MapFrom(src => src.DemandeId))
                .ForMember(dest => dest.Quantite, opt => opt.MapFrom(src => src.Quantite))
                .ForMember(dest => dest.NomMedicamnet, opt => opt.MapFrom(src => src.Medicament != null ? src.Medicament.Nom : null))
                .ForMember(dest => dest.DosageMedicament, opt => opt.MapFrom(src => src.Medicament != null ? src.Medicament.Dosage : null))
                .ForMember(dest => dest.PrixUnitaire, opt => opt.MapFrom(src => src.Medicament != null ? src.Medicament.PrixVente.ToString("F2") : "0"))
                .ForMember(dest => dest.PrixTotal, opt => opt.MapFrom(src => src.Medicament != null ? src.Quantite * src.Medicament.PrixVente : 0m));
        }
    }
}