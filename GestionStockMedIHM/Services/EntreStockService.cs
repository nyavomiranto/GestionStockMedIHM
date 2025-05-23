using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.EntreStocks;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Domain.DTOs.Stocks;
using GestionStockMedIHM.Interfaces.EntreStocks;
using GestionStockMedIHM.Interfaces.Fournisseurs;
using GestionStockMedIHM.Interfaces.Medicaments;
using GestionStockMedIHM.Interfaces.Stocks;
using GestionStockMedIHM.Models.Entities;
using GestionStockMedIHM.Repositories;

namespace GestionStockMedIHM.Services
{
    public class EntreStockService : IEntreStockService
    {
        private readonly IEntreStockRepository _entreStockRepository;
        private readonly IMedicamentService _mediicamentService;
        private readonly IFournisseurService _fournisseurService;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        public EntreStockService(IEntreStockRepository entreStockRepository, IMapper mapper, IMedicamentService mediicamentService, IFournisseurService fournisseurService, IStockService stockService)
        {
            _entreStockRepository = entreStockRepository;
            _mediicamentService = mediicamentService;
            _fournisseurService = fournisseurService;
            _stockService = stockService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<EntreStockResponseDto>> GetByIdAsync(int id)
        {
            try
            {
                var entreStock = await _entreStockRepository.GetByIdWithDetailsAsync(id);
                if (entreStock == null)
                {
                    return ApiResponse<EntreStockResponseDto>.ErrorResponse("Entrée de stock non trouvé");
                }
                var result = _mapper.Map<EntreStockResponseDto>(entreStock);
                return ApiResponse<EntreStockResponseDto>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<EntreStockResponseDto>.ErrorResponse(
                    "Erreur lors de la récupération de l'entrée du stock",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<IEnumerable<EntreStockResponseDto>>> GetAllAsync()
        {
            try
            {
                var entreStock = await _entreStockRepository.GetAllWithDetailsAsync();
                var result = _mapper.Map<IEnumerable<EntreStockResponseDto>>(entreStock);
                return ApiResponse<IEnumerable<EntreStockResponseDto>>.SuccessResponse(result);
            }
            catch (Exception ex) 
            {
                return ApiResponse<IEnumerable<EntreStockResponseDto>>.ErrorResponse(
                    "Erreur lors de la récupération des entrées de stocks",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<EntreStockResponseDto>> CreateAsync(CreateEntreStockDto createEntreStockDto)
        {
            try
            {
                var medicamentResponse = await _mediicamentService.GetByNomAsync(createEntreStockDto.NomMedicament);
                if (!medicamentResponse.Success || medicamentResponse.Data == null)
                {
                    return ApiResponse<EntreStockResponseDto>.ErrorResponse("Médicament non trouvé");
                }

                var fournisseurResponse = await _fournisseurService.GetByNomAsync(createEntreStockDto.NomFournisseur);
                if (!fournisseurResponse.Success || fournisseurResponse.Data == null)
                {
                    return ApiResponse<EntreStockResponseDto>.ErrorResponse("Fournisseur non trouvé");
                }

                var medicamentId = medicamentResponse.Data.Id;
                var datePeremption = DateTime.SpecifyKind(createEntreStockDto.DatePeremptionMedicament.Date, DateTimeKind.Utc); //conversion avant comparaison

                var stockResponse = await _stockService.GetByMedicamentAndDateRepemptionAsync(medicamentId, datePeremption);
                if (stockResponse.Success && stockResponse.Data != null)
                {
                    var updateStockDto = new UpdateStockDto
                    {
                        Quantite = stockResponse.Data.Quantite + createEntreStockDto.Quantite,
                        NomMedicament = createEntreStockDto.NomMedicament,
                        DatePeremption = datePeremption,
                    };
 
                    var updateResult = await _stockService.UpdateAsync(stockResponse.Data.Id, updateStockDto);
                    if (!updateResult.Success)
                    {
                        return ApiResponse<EntreStockResponseDto>.ErrorResponse("Erreur lors de la mise à jour du stock");
                    }
                }
                else
                {
                    var createStockDto = new CreateStockDto
                    {
                        NomMedicament = createEntreStockDto.NomMedicament,
                        DatePeremption = datePeremption,
                        Quantite = createEntreStockDto.Quantite
                    };

                    var createResult = await _stockService.CreateAsync(createStockDto);
                    if (!createResult.Success)
                    {
                        return ApiResponse<EntreStockResponseDto>.ErrorResponse("Erreur lors de la création du stock");
                    }
                }

                var entity = _mapper.Map<EntreStock>(createEntreStockDto);
                entity.MedicamentId = medicamentResponse.Data.Id;
                entity.FournisseurId = fournisseurResponse.Data.Id;

                await _entreStockRepository.AddAsync(entity);

                var resultDto = _mapper.Map<EntreStockResponseDto>(entity);
                return ApiResponse<EntreStockResponseDto>.SuccessResponse(resultDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<EntreStockResponseDto>.ErrorResponse(
                    "Erreur lors de la creation de l'entrée de stock",
                    new List<string> { ex.Message });

            }

        }

        public async Task<ApiResponse<EntreStockResponseDto>> UpdateAsync(int id, UpdateEntreStockDto updateDto)
        {
            try
            {
                // Récupérer l'entrée de stock existante
                var entreStock = await _entreStockRepository.GetByIdAsync(id);
                if (entreStock == null)
                {
                    return ApiResponse<EntreStockResponseDto>.ErrorResponse("Entrée de stock non trouvée");
                }
                // Valider la quantité
                if (updateDto.Quantite < 0)
                {
                    return ApiResponse<EntreStockResponseDto>.ErrorResponse("La quantité ne peut pas être négative");
                }

                // Valider la date de péremption
                if (updateDto.DatePeremptionMedicament.Date < DateTime.UtcNow.Date)
                {
                    return ApiResponse<EntreStockResponseDto>.ErrorResponse("La date de péremption ne peut pas être dans le passé");
                }

                // Vérifier le médicament
                var medicamentResponse = await _mediicamentService.GetByNomAsync(updateDto.NomMedicament);
                if (!medicamentResponse.Success || medicamentResponse.Data == null)
                {
                    return ApiResponse<EntreStockResponseDto>.ErrorResponse("Médicament non trouvé");
                }

                // Vérifier le fournisseur
                var fournisseurResponse = await _fournisseurService.GetByNomAsync(updateDto.NomFournisseur);
                if (!fournisseurResponse.Success || fournisseurResponse.Data == null)
                {
                    return ApiResponse<EntreStockResponseDto>.ErrorResponse("Fournisseur non trouvé");
                }

                // Normaliser la date de péremption
                var newDatePeremption = DateTime.SpecifyKind(updateDto.DatePeremptionMedicament.Date, DateTimeKind.Utc);

                // Trouver l'ancien stock
                var oldStockResponse = await _stockService.GetByMedicamentAndDateRepemptionAsync(entreStock.MedicamentId, entreStock.DatePeremptionMedicament);
                if (!oldStockResponse.Success || oldStockResponse.Data == null)
                {
                    return ApiResponse<EntreStockResponseDto>.ErrorResponse("Stock correspondant non trouvé");
                }

                // Si le médicament ou la date de péremption change
                if (entreStock.MedicamentId != medicamentResponse.Data.Id || entreStock.DatePeremptionMedicament != newDatePeremption)
                {
                    // Retirer la quantité de l'ancien stock
                    var oldStockNewQuantity = oldStockResponse.Data.Quantite - entreStock.Quantite;
                    if (oldStockNewQuantity < 0)
                    {
                        return ApiResponse<EntreStockResponseDto>.ErrorResponse("La modification rendrait la quantité de l'ancien stock négative");
                    }

                    if (oldStockNewQuantity == 0)
                    {
                        var deleteResult = await _stockService.DeleteAsync(oldStockResponse.Data.Id);
                        if (!deleteResult.Success)
                        {
                            return ApiResponse<EntreStockResponseDto>.ErrorResponse("Erreur lors de la suppression de l'ancien stock");
                        }
                        Console.WriteLine($"Ancien stock ID={oldStockResponse.Data.Id} supprimé car quantité = 0");
                    }
                    else
                    {
                        var updateOldStockDto = new UpdateStockDto
                        {
                            Quantite = oldStockNewQuantity,
                            NomMedicament = oldStockResponse.Data.NomMedicament,
                            DatePeremption = entreStock.DatePeremptionMedicament
                        };
                        var updateResult = await _stockService.UpdateAsync(oldStockResponse.Data.Id, updateOldStockDto);
                        if (!updateResult.Success)
                        {
                            return ApiResponse<EntreStockResponseDto>.ErrorResponse("Erreur lors de la mise à jour de l'ancien stock");
                        }
                    }

                    // Ajouter la quantité au nouveau stock
                    var newStockResponse = await _stockService.GetByMedicamentAndDateRepemptionAsync(medicamentResponse.Data.Id, newDatePeremption);
                    if (newStockResponse.Success && newStockResponse.Data != null)
                    {
                        // Mettre à jour le stock existant
                        var newStockNewQuantity = newStockResponse.Data.Quantite + updateDto.Quantite;
                        var updateNewStockDto = new UpdateStockDto
                        {
                            Quantite = newStockNewQuantity,
                            NomMedicament = updateDto.NomMedicament,
                            DatePeremption = newDatePeremption
                        };
                        var updateResult = await _stockService.UpdateAsync(newStockResponse.Data.Id, updateNewStockDto);
                        if (!updateResult.Success)
                        {
                            return ApiResponse<EntreStockResponseDto>.ErrorResponse("Erreur lors de la mise à jour du nouveau stock");
                        }
                    }
                    else
                    {
                        // Créer un nouveau stock
                        var createStockDto = new CreateStockDto
                        {
                            Quantite = updateDto.Quantite,
                            NomMedicament = updateDto.NomMedicament,
                            DatePeremption = newDatePeremption
                        };
                        var createResult = await _stockService.CreateAsync(createStockDto);
                        if (!createResult.Success)
                        {
                            return ApiResponse<EntreStockResponseDto>.ErrorResponse("Erreur lors de la création du nouveau stock");
                        }
                    }
                }
                else
                {
                    // Mise à jour de la quantité uniquement
                    var newStockQuantity = oldStockResponse.Data.Quantite + (updateDto.Quantite - entreStock.Quantite);
                    if (newStockQuantity < 0)
                    {
                        return ApiResponse<EntreStockResponseDto>.ErrorResponse("La modification rendrait la quantité du stock négative");
                    }

                    if (newStockQuantity == 0)
                    {
                        var deleteResult = await _stockService.DeleteAsync(oldStockResponse.Data.Id);
                        if (!deleteResult.Success)
                        {
                            return ApiResponse<EntreStockResponseDto>.ErrorResponse("Erreur lors de la suppression du stock");
                        }
                    }
                    else
                    {
                        var updateStockDto = new UpdateStockDto
                        {
                            Quantite = newStockQuantity,
                            NomMedicament = oldStockResponse.Data.NomMedicament,
                            DatePeremption = entreStock.DatePeremptionMedicament
                        };
                        var updateResult = await _stockService.UpdateAsync(oldStockResponse.Data.Id, updateStockDto);
                        if (!updateResult.Success)
                        {
                            return ApiResponse<EntreStockResponseDto>.ErrorResponse("Erreur lors de la mise à jour du stock");
                        }
                    }
                }

                // Mettre à jour l'entrée de stock
                entreStock.Quantite = updateDto.Quantite;
                entreStock.DatePeremptionMedicament = newDatePeremption;
                entreStock.MedicamentId = medicamentResponse.Data.Id;
                entreStock.FournisseurId = fournisseurResponse.Data.Id;
                entreStock.Motif = updateDto.Motif;
                entreStock.PrixUnitaire = updateDto.PrixUnitaire;
                await _entreStockRepository.UpdateAsync(entreStock);

                var resultDto = _mapper.Map<EntreStockResponseDto>(entreStock);
                return ApiResponse<EntreStockResponseDto>.SuccessResponse(resultDto, "Entrée de stock mise à jour avec succès");
            }
            catch (Exception ex)
            {
                return ApiResponse<EntreStockResponseDto>.ErrorResponse(
                    "Erreur lors de la mise à jour de l'entrée de stock",
                    new List<string> { ex.Message });
            }

        }
        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var entreStock = await _entreStockRepository.GetByIdAsync(id);
                if (entreStock == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Entrée de Stock non trouvé");
                }

                var stockResponse = await _stockService.GetByMedicamentAndDateRepemptionAsync(entreStock.MedicamentId, entreStock.DatePeremptionMedicament);

                if (!stockResponse.Success || stockResponse.Data == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Stock correspondant non trouvé");
                }
                else
                {
                    var newQuantity = stockResponse.Data.Quantite - entreStock.Quantite;
                    if (newQuantity < 0)
                    {
                        return ApiResponse<bool>.ErrorResponse("La suppression rendrait la quantité du stock négative");
                    }
                    if (newQuantity == 0)
                    {
                        var deleteResult = await _stockService.DeleteAsync(stockResponse.Data.Id);
                        if (!deleteResult.Success)
                        {
                            return ApiResponse<bool>.ErrorResponse("Erreur lors de la suppression du stock");
                        }
                    }
                    else 
                    {
                        var updateStockSto = new UpdateStockDto
                        {
                            Quantite = newQuantity,
                            NomMedicament = stockResponse.Data.NomMedicament,
                            DatePeremption = entreStock.DatePeremptionMedicament
                        };

                        var updatedResult = await _stockService.UpdateAsync(stockResponse.Data.Id, updateStockSto);
                        if (!updatedResult.Success)
                        {
                            return ApiResponse<bool>.ErrorResponse("Erreur lors de la mise à jour du stock");
                        }
                    }
                }

                    await _entreStockRepository.DeleteAsync(id);
                    return ApiResponse<bool>.SuccessResponse(true, "Entrée de stock supprimé avec succés");
               
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse(
                        "Erreur lors de la suppression de l'entrée de stock",
                        new List<string> { ex.Message });
            }
        }
    }

}

