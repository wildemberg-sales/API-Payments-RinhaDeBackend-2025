using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPaymentServices.Migrations
{
    /// <inheritdoc />
    public partial class payment_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Payments",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Payments",
                newName: "CorrelationId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Payments",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "CorrelationId",
                table: "Payments",
                newName: "Id");
        }
    }
}
