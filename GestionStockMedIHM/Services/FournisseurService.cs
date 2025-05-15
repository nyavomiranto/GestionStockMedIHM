using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Fournisseurs;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Interfaces.Fournisseurs;
using GestionStockMedIHM.Models.Entities;
using GestionStockMedIHM.Repositories;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace GestionStockMedIHM.Services
{
    public class FournisseurService: IFournisseurService
    {
        private readonly IFournisseurRepository _fournisseurRepository;
        private readonly IMapper _mapper;

        public FournisseurService(IFournisseurRepository fournisseurRepository, IMapper mapper)
        {
            _fournisseurRepository = fournisseurRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<FournisseurResponseDto>> GetByIdAsync(int id)
        {
            try
            {
                var fournisseur = await _fournisseurRepository.GetByIdAsync(id);
                if(fournisseur == null)
                {
                    return ApiResponse<FournisseurResponseDto>.ErrorResponse("Fournisseur non trouvé");
                }
                var result = _mapper.Map<FournisseurResponseDto>(fournisseur);
                return ApiResponse<FournisseurResponseDto>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<FournisseurResponseDto>.ErrorResponse(
                    "Erreur lors de la récupération du fournisseur",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<IEnumerable<FournisseurResponseDto>>> GetAllAsync()
        {
            try
            {
                var fournisseurs = await _fournisseurRepository.GetAllAsync();
                var result = _mapper.Map<IEnumerable<FournisseurResponseDto>>(fournisseurs);
                return ApiResponse<IEnumerable<FournisseurResponseDto>>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<FournisseurResponseDto>>.ErrorResponse(
                    "Erreur lors de la récupération des fournisseurs",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<FournisseurResponseDto>> CreateAsync(CreateFournisseurDto createFournisseurDto)
        {
            try
            {
                var fournisseur =_mapper.Map<Fournisseur>(createFournisseurDto);
                await _fournisseurRepository.AddAsync(fournisseur);

                var result = _mapper.Map<FournisseurResponseDto>(fournisseur);
                return ApiResponse<FournisseurResponseDto>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<FournisseurResponseDto>.ErrorResponse(
                    "Erreur lors de la creation du fournisseur",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<FournisseurResponseDto>> UpdateAsync(int id, UpdateFournisseurDto updateFournisseurDto)
        {
            try
            {
                var existingFournisseur = await _fournisseurRepository.GetByIdAsync(id);
                if (existingFournisseur == null)
                {
                    return ApiResponse<FournisseurResponseDto>.ErrorResponse("Fournisseur non trouvé");
                }

                _mapper.Map(updateFournisseurDto, existingFournisseur);
                await _fournisseurRepository.UpdateAsync(existingFournisseur);

                var result = _mapper.Map<FournisseurResponseDto>(existingFournisseur);
                return ApiResponse<FournisseurResponseDto>.SuccessResponse(result, "Fournisseur mise à jour avec succès");
            }
            catch (Exception ex)
            {
                return ApiResponse<FournisseurResponseDto>.ErrorResponse(
                      "Erreur lors de la mise à jour du fournisseur",
                      new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var existingFournisseur = await _fournisseurRepository.GetByIdAsync(id);
                if (existingFournisseur == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Fournisseur non trouvé");
                }

                await _fournisseurRepository.DeleteAsync(id);
                return ApiResponse<bool>.SuccessResponse(true, "Fournisseur supprimé avec succés");
            }
            catch(Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse(
                        "Erreur lors de la suppression du fournisseur",
                        new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<FournisseurResponseDto>> GetByNomAsync(string nom)
        {
            try
            {
                var fournisseur = await _fournisseurRepository.GetByNomAsync(nom);
                if (fournisseur == null)
                {
                    return ApiResponse<FournisseurResponseDto>.ErrorResponse($"Fournisseur avec le nom '{nom}' non trouvé");
                }
                var result = _mapper.Map<FournisseurResponseDto>(fournisseur);
                return ApiResponse<FournisseurResponseDto>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<FournisseurResponseDto>.ErrorResponse(
                    "Erreur lors de la récupération du fournisseur",
                    new List<string> { ex.Message });
            }

        }
    }
}
