using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleInsurance_VehicleInsuranceId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_VehicleInsuranceId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VehicleInsuranceId",
                table: "Vehicles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehicleInsuranceId",
                table: "Vehicles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleInsuranceId",
                table: "Vehicles",
                column: "VehicleInsuranceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleInsurance_VehicleInsuranceId",
                table: "Vehicles",
                column: "VehicleInsuranceId",
                principalTable: "VehicleInsurance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
