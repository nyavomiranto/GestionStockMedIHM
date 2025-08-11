using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Demandes;
using GestionStockMedIHM.Domain.DTOs.Responses;
using GestionStockMedIHM.Domain.DTOs.Stocks;
using GestionStockMedIHM.Domain.DTOs.Utilisateurs;
using GestionStockMedIHM.Interfaces.Utilisateurs;
using GestionStockMedIHM.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GestionStockMedIHM.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUtilisateurRepository _utilisateurRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(IUtilisateurRepository utilisateurRepository, IConfiguration configuration, IMapper mapper)
        {
            _utilisateurRepository = utilisateurRepository;
            _configuration = configuration;
            _mapper = mapper;
        }
    
       public async Task<ApiResponse<UtilisateurResponseDto>> RegisterAsync(UtilisateurRegisterDto utilisateurRegisterDto)
       {
            try
            {
                var emailExist = await _utilisateurRepository.EmailExistsAsync(utilisateurRegisterDto.Email);
                if (emailExist)
                {
                    return ApiResponse<UtilisateurResponseDto>.ErrorResponse("Cet Email est déja utilisé");
                }

                using var hmac = new HMACSHA512();
                var motDePasseSalt = hmac.Key;
                var motDePasseHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(utilisateurRegisterDto.MotDePasse));

                var utilisateur = _mapper.Map<Utilisateur>(utilisateurRegisterDto);
                utilisateur.MotDepasseSalt = motDePasseSalt;
                utilisateur.MotDePasseHash = motDePasseHash;

                await _utilisateurRepository.AddAsync(utilisateur);
                var result = _mapper.Map<UtilisateurResponseDto>(utilisateur);
                return ApiResponse<UtilisateurResponseDto>.SuccessResponse(result, "Inscription réussie. Votre compte est en attente d'approbation par un administrateur.");
            }
            catch (Exception ex) 
            {
                return ApiResponse<UtilisateurResponseDto>.ErrorResponse(
                       "Erreur lors de la creation d'un compte utilisateur",
                       new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<string>> LoginAsync(UtilisateurLoginDto utilisateurLoginDto)
        {
            try
            {
                var utilisateur = await _utilisateurRepository.GetByEmailAsync(utilisateurLoginDto.Email);
                if (utilisateur == null || !utilisateur.Etat)
                {
                    return ApiResponse<string>.ErrorResponse("Utilisateur non trouvé ou inactif.");
                }

                using var hmac = new HMACSHA512(utilisateur.MotDepasseSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(utilisateurLoginDto.MotDePasse));
                if (!computedHash.SequenceEqual(utilisateur.MotDePasseHash))
                {
                    return ApiResponse<string>.ErrorResponse("Mot de passe incorrect");
                }
                var token = GenerateJwtToken(utilisateur);
                return ApiResponse<string>.SuccessResponse(token, "Connexion réussie.");
            }
            catch (Exception ex) 
            {
                return ApiResponse<string>.ErrorResponse(
                           "Erreur lors de la creation de l'authentification",
                           new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<UtilisateurResponseDto>> UpdateUtilisateurEtatAsync(int IdUtilisateur, UtilisateurUpdateEtatDto utilisateurUpdateEtatDto)
        {
            try
            {
                var utilisateur = await _utilisateurRepository.GetByIdAsync(IdUtilisateur);
                if (utilisateur == null)
                {
                    return ApiResponse<UtilisateurResponseDto>.ErrorResponse("Utilisateur non trouvé.");
                }

                _mapper.Map(utilisateurUpdateEtatDto, utilisateur);
                await _utilisateurRepository.UpdateAsync(utilisateur);

                var result = _mapper.Map<UtilisateurResponseDto>(utilisateur);
                return ApiResponse<UtilisateurResponseDto>.SuccessResponse(result, "Etat de l'utilisateur mise à jour avec succès.");
            }
            catch (Exception ex) 
            {
                return ApiResponse<UtilisateurResponseDto>.ErrorResponse(
                               "Erreur lors de la mise à jour de l'état de l'utilisateur",
                               new List<string> { ex.Message });
            }
        }

        public string GenerateJwtToken(Utilisateur utilisateur)
        {
                var claims = new[]
           {
                new Claim(JwtRegisteredClaimNames.Sub, utilisateur.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, utilisateur.Id.ToString()),
                new Claim(ClaimTypes.Role, utilisateur.Role)
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(4),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ApiResponse<IEnumerable<UtilisateurResponseDto>>> GetAllAsync()
        {
            try
            {
                var utilisateurs = await _utilisateurRepository.GetAllAsync();
                var result = _mapper.Map<IEnumerable<UtilisateurResponseDto>>(utilisateurs);
                return ApiResponse<IEnumerable<UtilisateurResponseDto>>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<UtilisateurResponseDto>>.ErrorResponse(
                    "Erreur lors de la récupération des utilisateurs",
                    new List<string> { ex.Message });
            }
        }

    }
}
