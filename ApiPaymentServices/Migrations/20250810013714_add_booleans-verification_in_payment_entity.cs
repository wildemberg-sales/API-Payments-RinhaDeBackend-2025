using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPaymentServices.Migrations
{
    /// <inheritdoc />
    public partial class add_booleansverification_in_payment_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFallback",
                table: "Payments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "Payments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFallback",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "Payments");
        }
    }
}
