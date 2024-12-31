using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTradingApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "Users");
        }
    }
}
