using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Differencial.Repository.Migrations
{
    /// <inheritdoc />
    public partial class contratofix4_rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contrato_Produto_Id2",
                table: "Contrato");

            migrationBuilder.RenameColumn(
                name: "Id2",
                table: "Contrato",
                newName: "Id3");

            migrationBuilder.RenameIndex(
                name: "IX_Contrato_Id2",
                table: "Contrato",
                newName: "IX_Contrato_Id3");

            migrationBuilder.AddForeignKey(
                name: "FK_Contrato_Produto_Id3",
                table: "Contrato",
                column: "Id3",
                principalTable: "Produto",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contrato_Produto_Id3",
                table: "Contrato");

            migrationBuilder.RenameColumn(
                name: "Id3",
                table: "Contrato",
                newName: "Id2");

            migrationBuilder.RenameIndex(
                name: "IX_Contrato_Id3",
                table: "Contrato",
                newName: "IX_Contrato_Id2");

            migrationBuilder.AddForeignKey(
                name: "FK_Contrato_Produto_Id2",
                table: "Contrato",
                column: "Id2",
                principalTable: "Produto",
                principalColumn: "Id");
        }
    }
}
