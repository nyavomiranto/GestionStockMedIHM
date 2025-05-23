using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Notification;
using GestionStockMedIHM.Domain.Entities;

namespace GestionStockMedIHM.Profiles
{
    public class NotificationProfile: Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.UtilisateurId, opt => opt.MapFrom(src => src.UtilisateurId))
                .ForMember(dest => dest.DemandeId, opt => opt.MapFrom(src => src.DemandeId))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.EstVue, opt => opt.MapFrom(src => src.EstVue))
                .ForMember(dest => dest.DateNotification, opt => opt.MapFrom(src => src.DateNotification));

            CreateMap<NotificationDto, Notification>()
                .ForMember(dest => dest.Utilisateur, opt => opt.Ignore())
                .ForMember(dest => dest.Demande, opt => opt.Ignore());
        }
    }
}
