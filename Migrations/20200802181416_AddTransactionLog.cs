using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cinderella.Migrations
{
    public partial class AddTransactionLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionLogs",
                columns: table => new
                {
                    TransactionNumber = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLogs", x => x.TransactionNumber);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionLogs");
        }
    }
}
