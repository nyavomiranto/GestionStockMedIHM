﻿using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Fournisseurs;
using GestionStockMedIHM.Models.Entities;

namespace GestionStockMedIHM.Profiles
{
    public class FournisseurProfile: Profile
    {
        public FournisseurProfile() 
        {
            CreateMap<Fournisseur, FournisseurResponseDto>();
            CreateMap<CreateFournisseurDto, Fournisseur>();
            CreateMap<UpdateFournisseurDto, Fournisseur>();
        }
    }
}
