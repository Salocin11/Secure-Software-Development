using Microsoft.EntityFrameworkCore.Migrations;

namespace Philip.Migrations
{
    public partial class CommentMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinalComment",
                columns: table => new
                {
                    CommentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleID = table.Column<int>(nullable: false),
                    CommentName = table.Column<string>(maxLength: 32, nullable: false),
                    CommentWords = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalComment", x => x.CommentID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalComment");
        }
    }
}
