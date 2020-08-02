using Microsoft.EntityFrameworkCore.Migrations;

namespace Cinderella.Migrations
{
    public partial class EditAuditRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeyShoeFieldID",
                table: "AuditRecords");

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "AuditRecords",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desc",
                table: "AuditRecords");

            migrationBuilder.AddColumn<int>(
                name: "KeyShoeFieldID",
                table: "AuditRecords",
                nullable: false,
                defaultValue: 0);
        }
    }
}
