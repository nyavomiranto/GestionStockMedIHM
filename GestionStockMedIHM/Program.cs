using AutoMapper;
using GestionStockMedIHM.Domain.DTOs.Utilisateurs;
using GestionStockMedIHM.Interfaces;
using GestionStockMedIHM.Interfaces.Demandes;
using GestionStockMedIHM.Interfaces.EntreStocks;
using GestionStockMedIHM.Interfaces.Fournisseurs;
using GestionStockMedIHM.Interfaces.LigneDemandes;
using GestionStockMedIHM.Interfaces.Medicaments;
using GestionStockMedIHM.Interfaces.Notifications;
using GestionStockMedIHM.Interfaces.Stocks;
using GestionStockMedIHM.Interfaces.Utilisateurs;
using GestionStockMedIHM.Profiles;
using GestionStockMedIHM.Middleware;
using GestionStockMedIHM.Models.Entities;
using GestionStockMedIHM.Repositories;
using GestionStockMedIHM.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using GestionStockMedIHM.Hubs;
using System.Security.Claims;
using GestionStockMedIHM.Interfaces.SortieStocks;
using GestionStockMedIHM.Interfaces.LigneSortieStocks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

// SignalR pour gérer les connexions en temps réel
builder.Services.AddSignalR();

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
    // Mappage explicite des claims
    options.TokenValidationParameters.NameClaimType = ClaimTypes.Name; // "sub" -> ClaimTypes.Name
    options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role; // "role" -> ClaimTypes.Role
    options.MapInboundClaims = false; // Désactiver le mappage automatique pour contrôler manuellement
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            var nameIdentifier = claimsIdentity?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (!string.IsNullOrEmpty(nameIdentifier))
            {
                claimsIdentity?.AddClaim(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
            }
            return Task.CompletedTask;
        }
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
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ISortieStockRepository, SortieStockRepository>();
builder.Services.AddScoped<ILigneSortieStockRepository, LigneSortieStockRepository>();


// Services
builder.Services.AddScoped<IMedicamentService, MedicamentService>();
builder.Services.AddScoped<IFournisseurService, FournisseurService>();
builder.Services.AddScoped<IEntreStockService, EntreStockService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IDemandeService, DemandeService>();
builder.Services.AddScoped<ILigneDemandeService, LigneDemandeService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ISortieStockService, SortieStockService>();
builder.Services.AddScoped<ILigneSortieStockService, LigneSortieStockService>();



// AutoMapper
builder.Services.AddAutoMapper(typeof(MedicamentProfile),
    typeof(FournisseurProfile),
    typeof(EntreStockProfile),
    typeof(StockProfile),
    typeof(DemandeProfile),
    typeof(LigneDemandeProfile),
    typeof(SortieStockProfile),
    typeof(LigneSortieStockProfile));

// Swagger/API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

// Ajouter CORS avec des origines spécifiques
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5500",
            "http://localhost:3000",
            "https://localhost:7191",
            "http://127.0.0.1:5501",
            "http://localhost:5501") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigins"); // CORS
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<NotificationHub>("/notificationHub");
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
            admin.Etat = true;

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