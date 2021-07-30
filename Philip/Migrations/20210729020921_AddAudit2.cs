using Microsoft.EntityFrameworkCore.Migrations;

namespace Philip.Migrations
{
    public partial class AddAudit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewValue",
                table: "AuditRecords",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldValue",
                table: "AuditRecords",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewValue",
                table: "AuditRecords");

            migrationBuilder.DropColumn(
                name: "OldValue",
                table: "AuditRecords");
        }
    }
}
