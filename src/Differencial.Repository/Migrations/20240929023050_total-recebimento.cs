using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Differencial.Repository.Migrations
{
    /// <inheritdoc />
    public partial class totalrecebimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IndFaturado",
                table: "LancamentoFinanceiroTotal",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IndLiquidado",
                table: "LancamentoFinanceiroTotal",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndFaturado",
                table: "LancamentoFinanceiroTotal");

            migrationBuilder.DropColumn(
                name: "IndLiquidado",
                table: "LancamentoFinanceiroTotal");
        }
    }
}
