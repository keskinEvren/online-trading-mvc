using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTradingApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateasdasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Transactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Transactions",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
