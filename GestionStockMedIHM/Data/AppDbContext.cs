using System;
using System.Collections.Generic;
using GestionStockMedIHM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionStockMedIHM.Models.Entities;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Demande> Demandes { get; set; }
    public DbSet<EntreStock> EntreStocks { get; set; }
    public DbSet<Fournisseur> Fournisseurs { get; set; }
    public DbSet<LigneDemande> LigneDemandes { get; set; }
    public DbSet<LigneSortieStock> LigneSortieStocks { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<SortieStock> SortieStocks { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Utilisateur> Utilisateurs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurer la relation entre Demande et LigneDemande
        modelBuilder.Entity<Demande>()
            .HasMany(d => d.LignesDemande)
            .WithOne(ld => ld.Demande)
            .HasForeignKey(ld => ld.DemandeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configurer la relation entre LigneDemande et Medicament
        modelBuilder.Entity<LigneDemande>()
            .HasOne(ld => ld.Medicament)
            .WithMany()
            .HasForeignKey(ld => ld.MedicamentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configurer la relation entre Demande et Utilisateur
        modelBuilder.Entity<Demande>()
            .HasOne(d => d.Utilisateur)
            .WithMany(u => u.Demandes)
            .HasForeignKey(d => d.UtilisateurId)
            .OnDelete(DeleteBehavior.Restrict);

        //Configurer l'etat a false au debut
        modelBuilder.Entity<Utilisateur>()
            .Property(u => u.Etat)
            .HasDefaultValue(false);

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
