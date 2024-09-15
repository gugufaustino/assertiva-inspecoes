using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Differencial.Repository.Migrations
{
    /// <inheritdoc />
    public partial class contratofix4_removekey2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContratoLancamento_Contrato_IdContrato",
                table: "ContratoLancamento");

            migrationBuilder.DropForeignKey(
                 name: "FK_Contrato_Produto_Id3",
                 table: "Contrato");
 

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contrato",
                table: "Contrato");
 
            migrationBuilder.DropIndex(
                name: "IX_Contrato_Id3",
                table: "Contrato"); 

            migrationBuilder.DropColumn(
               name: "Id",
               table: "Contrato");

            migrationBuilder.Sql("EXEC sp_rename 'Contrato.Id3', 'Id', 'COLUMN'");

        

            migrationBuilder.AddPrimaryKey(
             name: "PK_Contrato",
             table: "Contrato",
             column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contrato_Produto_Id",
                table: "Contrato",
                column: "Id",
                principalTable: "Produto",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContratoLancamento_Contrato_IdContrato",
                table: "ContratoLancamento",
                column: "IdContrato",
                principalTable: "Contrato",
                principalColumn: "Id");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
         
        }
    }
}
