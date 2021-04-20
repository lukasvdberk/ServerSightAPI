using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerSightAPI.Migrations
{
    public partial class TitelToNameServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                "Title",
                "Servers",
                "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                "Name",
                "Servers",
                "Title");
        }
    }
}