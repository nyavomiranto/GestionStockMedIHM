using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionStockMedIHM.Migrations
{
    /// <inheritdoc />
    public partial class RestrictMedicamentDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LigneDemandes_Medicaments_MedicamentId",
                table: "LigneDemandes");

            migrationBuilder.AddForeignKey(
                name: "FK_LigneDemandes_Medicaments_MedicamentId",
                table: "LigneDemandes",
                column: "MedicamentId",
                principalTable: "Medicaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LigneDemandes_Medicaments_MedicamentId",
                table: "LigneDemandes");

            migrationBuilder.AddForeignKey(
                name: "FK_LigneDemandes_Medicaments_MedicamentId",
                table: "LigneDemandes",
                column: "MedicamentId",
                principalTable: "Medicaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
