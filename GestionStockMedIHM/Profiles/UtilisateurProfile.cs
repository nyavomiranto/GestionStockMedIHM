using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Utilisateurs;
using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Profiles
{
    public class UtilisateurProfile: Profile
    {
        public UtilisateurProfile() 
        {
            CreateMap<UtilisateurRegisterDto, Utilisateur>()
                .ForMember(dest => dest.MotDePasseHash, opt => opt.Ignore())
                .ForMember(dest => dest.MotDepasseSalt, opt => opt.Ignore())
                .ForMember(dest => dest.Etat, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Demandes, opt => opt.Ignore());

            CreateMap<Utilisateur, UtilisateurResponseDto>();

            CreateMap<UtilisateurUpdateEtatDto, Utilisateur>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.Nom, opt => opt.Ignore())
                .ForMember(dest => dest.MotDePasseHash, opt => opt.Ignore())
                .ForMember(dest => dest.MotDepasseSalt, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.Demandes, opt => opt.Ignore());
        }
    }
}
