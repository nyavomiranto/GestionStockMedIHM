﻿// <auto-generated />
using System;
using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GestionStockMedIHM.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250516140731_UtilisateurEtat")]
    partial class UtilisateurEtat
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GestionStockMedIHM.Domain.Entities.LigneDemande", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DemandeId")
                        .HasColumnType("integer");

                    b.Property<int>("MedicamentId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantite")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DemandeId");

                    b.HasIndex("MedicamentId");

                    b.ToTable("LigneDemandes");
                });

            modelBuilder.Entity("GestionStockMedIHM.Domain.Entities.LigneSortieStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DemandeId")
                        .HasColumnType("integer");

                    b.Property<int>("MedicamentId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantite")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DemandeId");

                    b.HasIndex("MedicamentId");

                    b.ToTable("LigneSortieStocks");
                });

            modelBuilder.Entity("GestionStockMedIHM.Domain.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateNotification")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DemandeId")
                        .HasColumnType("integer");

                    b.Property<bool>("EstVue")
                        .HasColumnType("boolean");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UtilisateurId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DemandeId");

                    b.HasIndex("UtilisateurId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("GestionStockMedIHM.Models.Entities.Demande", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateDemande")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NomClient")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StatutDemande")
                        .HasColumnType("integer");

                    b.Property<int>("UtilisateurId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UtilisateurId");

                    b.ToTable("Demandes");
                });

            modelBuilder.Entity("GestionStockMedIHM.Models.Entities.EntreStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateEntre")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DatePeremptionMedicament")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FournisseurId")
                        .HasColumnType("integer");

                    b.Property<int>("MedicamentId")
                        .HasColumnType("integer");

                    b.Property<string>("Motif")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PrixUnitaire")
                        .HasColumnType("integer");

                    b.Property<int>("Quantite")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FournisseurId");

                    b.HasIndex("MedicamentId");

                    b.ToTable("EntreStocks");
                });

            modelBuilder.Entity("GestionStockMedIHM.Models.Entities.Fournisseur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Adresse")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Fournisseurs");
                });

            modelBuilder.Entity("GestionStockMedIHM.Models.Entities.Medicament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Dosage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Forme")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PrixVente")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Medicaments");
                });

            modelBuilder.Entity("GestionStockMedIHM.Models.Entities.SortieStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateSortie")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DemandeId")
                        .HasColumnType("integer");

                    b.Property<int>("UtilisateurId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DemandeId");

                    b.HasIndex("UtilisateurId");

                    b.ToTable("SortieStocks");
                });

            modelBuilder.Entity("GestionStockMedIHM.Models.Entities.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DatePeremption")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MedicamentId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantite")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MedicamentId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("GestionStockMedIHM.Models.Entities.Utilisateur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Etat")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<byte[]>("MotDePasseHash")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("MotDepasseSalt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Utilisateurs");
                });

            modelBuilder.Entity("GestionStockMedIHM.Domain.Entities.LigneDemande", b =>
                {
                    b.HasOne("GestionStockMedIHM.Models.Entities.Demande", "Demande")
                        .WithMany("LignesDemande")
                        .HasForeignKey("DemandeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestionStockMedIHM.Models.Entities.Medicament", "Medicament")
                        .WithMany()
                        .HasForeignKey("MedicamentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Demande");

                    b.Navigation("Medicament");
                });

            modelBuilder.Entity("GestionStockMedIHM.Domain.Entities.LigneSortieStock", b =>
                {
                    b.HasOne("GestionStockMedIHM.Models.Entities.Demande", "Demande")
                        .WithMany()
                        .HasForeignKey("DemandeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestionStockMedIHM.Models.Entities.Medicament", "Medicament")
                        .WithMany()
                        .HasForeignKey("MedicamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Demande");

                    b.Navigation("Medicament");
                });

            modelBuilder.Entity("GestionStockMedIHM.Domain.Entities.Notification", b =>
                {
                    b.HasOne("GestionStockMedIHM.Models.Entities.Demande", "Demande")
                        .WithMany()
                        .HasForeignKey("DemandeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestionStockMedIHM.Models.Entities.Utilisateur", "Utilisateur")
                        .WithMany()
                        .HasForeignKey("UtilisateurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Demande");

                    b.Navigation("Utilisateur");
                });

            modelBuilder.Entity("GestionStockMedIHM.Models.Entities.Demande", b =>
                {
                    b.HasOne("GestionStockMedIHM.Models.Entities.Utilisateur", "Utilisateur")
                        .WithMany("Demandes")
                        .HasForeignKey("UtilisateurId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Utilisateur");
                });

            modelBuilder.Entity("GestionStockMedIHM.Models.Entities.EntreStock", b =>
                {
                    b.HasOne("GestionStockMedIHM.Models.Entities.Fournisseur", "Fournisseur")
                        .WithMany()
                        .HasForeignKey("FournisseurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestionStockMedIHM.Models.Entities.Medicament", "Medicament")
                        .WithMany()
                        .HasForeignKey("MedicamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fournisseur");

                    b.Navigation("Medicament");
                });

            modelBuilder.Entity("GestionStockMedIHM.Models.Entities.SortieStock", b =>
                {
                    b.HasOne("GestionStockMedIHM.Models.Entities.Demande", "Demande")
                        .WithMany()
                        .HasForeignKey("DemandeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestionStockMedIHM.Models.Entities.Utilisateur", "Utilisateur")
                        .WithMany()
                        .HasForeignKey("UtilisateurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Demande");

                    b.Navigation("Utilisateur");
                });

            modelBuilder.Entity("GestionStockMedIHM.Models.Entities.Stock", b =>
                {
                    b.HasOne("GestionStockMedIHM.Models.Entities.Medicament", "Medicament")
                        .WithMany()
                        .HasForeignKey("MedicamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medicament");
                });

            modelBuilder.Entity("GestionStockMedIHM.Models.Entities.Demande", b =>
                {
                    b.Navigation("LignesDemande");
                });

            modelBuilder.Entity("GestionStockMedIHM.Models.Entities.Utilisateur", b =>
                {
                    b.Navigation("Demandes");
                });
#pragma warning restore 612, 618
        }
    }
}
