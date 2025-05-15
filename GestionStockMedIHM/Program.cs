using GestionStockMedIHM.Interfaces;
using GestionStockMedIHM.Interfaces.Demandes;
using GestionStockMedIHM.Interfaces.EntreStocks;
using GestionStockMedIHM.Interfaces.Fournisseurs;
using GestionStockMedIHM.Interfaces.LigneDemandes;
using GestionStockMedIHM.Interfaces.Medicaments;
using GestionStockMedIHM.Interfaces.Stocks;
using GestionStockMedIHM.Mappings;
using GestionStockMedIHM.Middleware;
using GestionStockMedIHM.Models.Entities;
using GestionStockMedIHM.Repositories;
using GestionStockMedIHM.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging =>
{
    logging.AddConsole(); // Assure que les logs s'affichent dans la console
    logging.AddDebug();   // Assure que les logs s'affichent dans la fenêtre de sortie de Visual Studio
});

// Base de données
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

// Enregistrement générique
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IMedicamentRepository, MedicamentRepository>();
builder.Services.AddScoped<IFournisseurRepository, FournisseurRepository>();
builder.Services.AddScoped<IEntreStockRepository, EntreStockRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IDemandeRepository, DemandeRepository>();
builder.Services.AddScoped<ILigneDemandeRepository, LigneDemandeRepository>();

// Services
builder.Services.AddScoped<IMedicamentService, MedicamentService>();
builder.Services.AddScoped<IFournisseurService, FournisseurService>();
builder.Services.AddScoped<IEntreStockService, EntreStockService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IDemandeService, DemandeService>();
builder.Services.AddScoped<ILigneDemandeService, LigneDemandeService>();

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
app.UseAuthorization();
app.MapControllers();
app.Run();