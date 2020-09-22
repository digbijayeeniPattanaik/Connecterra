using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UpdateAuditTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Audits",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Audits");

            migrationBuilder.AddColumn<int>(
                name: "AuditId",
                table: "Audits",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Audits",
                table: "Audits",
                column: "AuditId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Audits",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "AuditId",
                table: "Audits");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Audits",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Audits",
                table: "Audits",
                column: "Id");
        }
    }
}
