using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionStockMedIHM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSortieStockModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LigneSortieStocks_Demandes_DemandeId",
                table: "LigneSortieStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_LigneSortieStocks_Medicaments_MedicamentId",
                table: "LigneSortieStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_SortieStocks_Utilisateurs_UtilisateurId",
                table: "SortieStocks");

            migrationBuilder.RenameColumn(
                name: "DemandeId",
                table: "LigneSortieStocks",
                newName: "SortieStockId");

            migrationBuilder.RenameIndex(
                name: "IX_LigneSortieStocks_DemandeId",
                table: "LigneSortieStocks",
                newName: "IX_LigneSortieStocks_SortieStockId");

            migrationBuilder.AddForeignKey(
                name: "FK_LigneSortieStocks_Medicaments_MedicamentId",
                table: "LigneSortieStocks",
                column: "MedicamentId",
                principalTable: "Medicaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LigneSortieStocks_SortieStocks_SortieStockId",
                table: "LigneSortieStocks",
                column: "SortieStockId",
                principalTable: "SortieStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SortieStocks_Utilisateurs_UtilisateurId",
                table: "SortieStocks",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LigneSortieStocks_Medicaments_MedicamentId",
                table: "LigneSortieStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_LigneSortieStocks_SortieStocks_SortieStockId",
                table: "LigneSortieStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_SortieStocks_Utilisateurs_UtilisateurId",
                table: "SortieStocks");

            migrationBuilder.RenameColumn(
                name: "SortieStockId",
                table: "LigneSortieStocks",
                newName: "DemandeId");

            migrationBuilder.RenameIndex(
                name: "IX_LigneSortieStocks_SortieStockId",
                table: "LigneSortieStocks",
                newName: "IX_LigneSortieStocks_DemandeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LigneSortieStocks_Demandes_DemandeId",
                table: "LigneSortieStocks",
                column: "DemandeId",
                principalTable: "Demandes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LigneSortieStocks_Medicaments_MedicamentId",
                table: "LigneSortieStocks",
                column: "MedicamentId",
                principalTable: "Medicaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SortieStocks_Utilisateurs_UtilisateurId",
                table: "SortieStocks",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
