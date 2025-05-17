using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Utilisateurs;
using GestionStockMedIHM.Interfaces;
using GestionStockMedIHM.Interfaces.Demandes;
using GestionStockMedIHM.Interfaces.EntreStocks;
using GestionStockMedIHM.Interfaces.Fournisseurs;
using GestionStockMedIHM.Interfaces.LigneDemandes;
using GestionStockMedIHM.Interfaces.Medicaments;
using GestionStockMedIHM.Interfaces.Stocks;
using GestionStockMedIHM.Interfaces.Utilisateurs;
using GestionStockMedIHM.Mappings;
using GestionStockMedIHM.Middleware;
using GestionStockMedIHM.Models.Entities;
using GestionStockMedIHM.Repositories;
using GestionStockMedIHM.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();  
});

// Base de données
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

// Configuration Authentification JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
    


// Enregistrement générique
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IMedicamentRepository, MedicamentRepository>();
builder.Services.AddScoped<IFournisseurRepository, FournisseurRepository>();
builder.Services.AddScoped<IEntreStockRepository, EntreStockRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IDemandeRepository, DemandeRepository>();
builder.Services.AddScoped<ILigneDemandeRepository, LigneDemandeRepository>();
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();

// Services
builder.Services.AddScoped<IMedicamentService, MedicamentService>();
builder.Services.AddScoped<IFournisseurService, FournisseurService>();
builder.Services.AddScoped<IEntreStockService, EntreStockService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IDemandeService, DemandeService>();
builder.Services.AddScoped<ILigneDemandeService, LigneDemandeService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MedicamentProfile), 
    typeof(FournisseurProfile), 
    typeof(EntreStockProfile),
    typeof(StockProfile),
    typeof(DemandeProfile),
    typeof(LigneDemandeProfile));

// Swagger/API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await InitializeAdmin(app.Services);

app.Run();

async Task InitializeAdmin(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var utilisateurRepository = scope.ServiceProvider.GetRequiredService<IUtilisateurRepository>();
    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        if (!await utilisateurRepository.EmailExistsAsync("admin@example.com"))
        {
            using var hmac = new HMACSHA512();
            var motDePasseSalt = hmac.Key;
            var motDePasseHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Admin123!"));

            var adminDto = new UtilisateurRegisterDto
            {
                Email = "admin@example.com",
                Nom = "Admin",
                MotDePasse = "Admin123!",
                Role = "Admin"
            };

            var admin = mapper.Map<Utilisateur>(adminDto);
            admin.MotDepasseSalt = motDePasseSalt;
            admin.MotDePasseHash = motDePasseHash;
            admin.Etat = true; // Admin actif

            await utilisateurRepository.AddAsync(admin);
            logger.LogInformation("Utilisateur admin créé avec succès.");
        }
        else
        {
            logger.LogInformation("Utilisateur admin existe déjà.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erreur lors de l'initialisation de l'admin.");
    }
}