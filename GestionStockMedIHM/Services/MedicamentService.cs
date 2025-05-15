using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Medicaments;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Interfaces.Medicaments;
using GestionStockMedIHM.Models.Entities;
using GestionStockMedIHM.Repositories;

namespace GestionStockMedIHM.Services
{
    public class MedicamentService: IMedicamentService
    {
        private readonly IMedicamentRepository _medicamentRepository;
        private readonly IMapper _mapper;

        public MedicamentService (IMedicamentRepository medicamentRepository, IMapper mapper)
        {
            _mapper = mapper;
            _medicamentRepository = medicamentRepository;
        }

        public async Task<ApiResponse<MedicamentDto>> GetByIdAsync (int id)
        {
            try
            {
                var medicament = await _medicamentRepository.GetByIdAsync(id);
                if (medicament == null)
                {
                    return ApiResponse<MedicamentDto>.ErrorResponse("Médicament non trouvé");
                }
                var result = _mapper.Map<MedicamentDto>(medicament);
                return ApiResponse<MedicamentDto>.SuccessResponse(result);
            }
            catch (Exception ex) 
            {
                return ApiResponse<MedicamentDto>.ErrorResponse(
                    "Erreur lors de la récupération du médicament",
                    new List<string> { ex.Message });
            }

        }

        public async Task<ApiResponse<IEnumerable<MedicamentDto>>> GetAllAsync()
        {
            try
            {
                var medicaments = await _medicamentRepository.GetAllAsync();
                var result = _mapper.Map<IEnumerable<MedicamentDto>>(medicaments);
                return ApiResponse<IEnumerable<MedicamentDto>>.SuccessResponse(result);
            }
            catch (Exception ex) 
            {
                return ApiResponse<IEnumerable<MedicamentDto>>.ErrorResponse(
                "Erreur lors de la récupération des médicaments",
                new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<MedicamentDto>> CreateAsync(CreateMedicamentDto createMedicamentDto)
        {
            try
            {
                var medicament = _mapper.Map<Medicament>(createMedicamentDto);
                await _medicamentRepository.AddAsync(medicament);

                var result = _mapper.Map<MedicamentDto>(medicament);
                return ApiResponse<MedicamentDto>.SuccessResponse(result, "Médicament créé avec succès");
            }
            catch (Exception ex) 
            {
                return ApiResponse<MedicamentDto>.ErrorResponse(
                   "Erreur lors de la création du médicament",
                   new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<MedicamentDto>> UpdateAsync(int id, UpdateMedicamentDto updateMedicamentDto)
        {
            try
            {
                var existingMedicament = await _medicamentRepository.GetByIdAsync(id);
                if (existingMedicament == null)
                {
                    return ApiResponse<MedicamentDto>.ErrorResponse("Médicament non trouvé");
                }

                _mapper.Map(updateMedicamentDto, existingMedicament);
                await _medicamentRepository.UpdateAsync(existingMedicament);

                var result = _mapper.Map<MedicamentDto>(existingMedicament);
                return ApiResponse<MedicamentDto>.SuccessResponse(result, "Médicament mis à jour avec succès");
            }
            catch (Exception ex) 
            {
                return ApiResponse<MedicamentDto>.ErrorResponse(
                   "Erreur lors de la mise à jour du médicament",
                   new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var medicament = await _medicamentRepository.GetByIdAsync(id);
                if (medicament == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Médicament non trouvé");
                }

                await _medicamentRepository.DeleteAsync(id);
                return ApiResponse<bool>.SuccessResponse(true, "Médicament supprimé avec succés");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse(
                    "Erreur lors de la suppression du médicament",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<MedicamentDto>> GetByNomAsync(string nom)
        {
            try
            {
                var medicament = await _medicamentRepository.GetByNomAsync(nom);
                if (medicament == null)
                {
                    return ApiResponse<MedicamentDto>.ErrorResponse($"Médicament avec le nom '{nom}' non trouvé");
                }
                var result = _mapper.Map<MedicamentDto>(medicament);
                return ApiResponse<MedicamentDto>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<MedicamentDto>.ErrorResponse(
                    "Erreur lors de la récupération du médicament",
                    new List<string> { ex.Message });
            }

        }
    }
}
