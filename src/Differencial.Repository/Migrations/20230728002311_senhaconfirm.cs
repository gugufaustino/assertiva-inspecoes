using Microsoft.EntityFrameworkCore.Migrations;

namespace Differencial.Repository.Migrations
{
    public partial class senhaconfirm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenhaConfirmacao",
                table: "Operador",
                type: "varchar(250)",
                unicode: false,
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenhaConfirmacao",
                table: "Operador");
        }
    }
}
