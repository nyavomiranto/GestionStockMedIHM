using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Medicaments;
using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Profiles
{
    public class MedicamentProfile: Profile
    {
        public MedicamentProfile() 
        {
            CreateMap<Medicament, MedicamentDto>();
            CreateMap<CreateMedicamentDto, Medicament>();
            CreateMap<UpdateMedicamentDto, Medicament>();
        }

    }
}
