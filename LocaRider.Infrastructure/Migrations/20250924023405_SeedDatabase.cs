using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LocaRider.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RentalPlans",
                columns: new[] { "RentalPlanId", "DailyPrice", "RentalPlanPeriodInDays" },
                values: new object[,]
                {
                    { "1", 30m, 7 },
                    { "2", 28m, 15 },
                    { "3", 22m, 30 },
                    { "4", 20m, 45 },
                    { "5", 18m, 50 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RentalPlans",
                keyColumn: "RentalPlanId",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "RentalPlans",
                keyColumn: "RentalPlanId",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "RentalPlans",
                keyColumn: "RentalPlanId",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "RentalPlans",
                keyColumn: "RentalPlanId",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "RentalPlans",
                keyColumn: "RentalPlanId",
                keyValue: "5");
        }
    }
}
