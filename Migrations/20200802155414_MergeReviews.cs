using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cinderella.Migrations
{
    public partial class MergeReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reviewDescs");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.CreateTable(
                name: "ReviewFinals",
                columns: table => new
                {
                    ReviewID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ShoeID = table.Column<int>(nullable: false),
                    ReviewName = table.Column<string>(maxLength: 32, nullable: true),
                    ReviewWords = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewFinals", x => x.ReviewID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewFinals");

            migrationBuilder.CreateTable(
                name: "reviewDescs",
                columns: table => new
                {
                    ReviewDescID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReviewID = table.Column<int>(nullable: false),
                    ReviewName = table.Column<string>(nullable: true),
                    ReviewWords = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviewDescs", x => x.ReviewDescID);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    ReviewID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ShoeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.ReviewID);
                });
        }
    }
}
