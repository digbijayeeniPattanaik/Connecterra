using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UpdateFarm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cows_Farm_FarmId",
                table: "Cows");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Farm_FarmId",
                table: "Sensors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Farm",
                table: "Farm");

            migrationBuilder.RenameTable(
                name: "Farm",
                newName: "Farms");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Farms",
                table: "Farms",
                column: "FarmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cows_Farms_FarmId",
                table: "Cows",
                column: "FarmId",
                principalTable: "Farms",
                principalColumn: "FarmId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Farms_FarmId",
                table: "Sensors",
                column: "FarmId",
                principalTable: "Farms",
                principalColumn: "FarmId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cows_Farms_FarmId",
                table: "Cows");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Farms_FarmId",
                table: "Sensors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Farms",
                table: "Farms");

            migrationBuilder.RenameTable(
                name: "Farms",
                newName: "Farm");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Farm",
                table: "Farm",
                column: "FarmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cows_Farm_FarmId",
                table: "Cows",
                column: "FarmId",
                principalTable: "Farm",
                principalColumn: "FarmId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Farm_FarmId",
                table: "Sensors",
                column: "FarmId",
                principalTable: "Farm",
                principalColumn: "FarmId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
