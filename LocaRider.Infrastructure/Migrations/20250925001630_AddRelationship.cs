using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocaRider.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DevolutionDate",
                table: "Rentals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Rentals",
                type: "numeric",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_DriverId",
                table: "Rentals",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_MotorcycleId",
                table: "Rentals",
                column: "MotorcycleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Drivers_DriverId",
                table: "Rentals",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "DriverId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Motorcycles_MotorcycleId",
                table: "Rentals",
                column: "MotorcycleId",
                principalTable: "Motorcycles",
                principalColumn: "MotorcycleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Drivers_DriverId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Motorcycles_MotorcycleId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_DriverId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_MotorcycleId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "DevolutionDate",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Rentals");
        }
    }
}
