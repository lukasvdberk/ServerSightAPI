using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerSightAPI.Migrations
{
    public partial class AddedPortsToServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "PortServers",
                table => new
                {
                    Id = table.Column<string>("text", nullable: false),
                    Port = table.Column<int>("integer", nullable: false),
                    ServerId = table.Column<string>("text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortServers", x => x.Id);
                    table.ForeignKey(
                        "FK_PortServers_Servers_ServerId",
                        x => x.ServerId,
                        "Servers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_PortServers_ServerId",
                "PortServers",
                "ServerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "PortServers");
        }
    }
}