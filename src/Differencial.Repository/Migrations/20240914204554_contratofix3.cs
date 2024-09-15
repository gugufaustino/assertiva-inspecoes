using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Differencial.Repository.Migrations
{
    /// <inheritdoc />
    public partial class contratofix3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contrato_Produto_Id",
                table: "Contrato");

            migrationBuilder.AddColumn<int>(
                name: "Id2",
                table: "Contrato",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // preciso de um update aqui para q Id2 receba valores de Id antes de criar a uniq index
            migrationBuilder.Sql("UPDATE Contrato SET Id2 = Id");
            migrationBuilder.CreateIndex(
                name: "IX_Contrato_Id2",
                table: "Contrato",
                column: "Id2",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contrato_Produto_Id2",
                table: "Contrato",
                column: "Id2",
                principalTable: "Produto",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contrato_Produto_Id2",
                table: "Contrato");

            migrationBuilder.DropIndex(
                name: "IX_Contrato_Id2",
                table: "Contrato");

            migrationBuilder.DropColumn(
                name: "Id2",
                table: "Contrato");

            migrationBuilder.AddForeignKey(
                name: "FK_Contrato_Produto_Id",
                table: "Contrato",
                column: "Id",
                principalTable: "Produto",
                principalColumn: "Id");
        }
    }
}
