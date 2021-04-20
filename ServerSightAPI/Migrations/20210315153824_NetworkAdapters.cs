using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerSightAPI.Migrations
{
    public partial class NetworkAdapters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "NetworkAdapterServers",
                table => new
                {
                    Id = table.Column<string>("text", nullable: false),
                    AdapterName = table.Column<string>("text", nullable: true),
                    Ip = table.Column<string>("text", nullable: true),
                    ServerId = table.Column<string>("text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkAdapterServers", x => x.Id);
                    table.ForeignKey(
                        "FK_NetworkAdapterServers_AspNetUsers_ServerId",
                        x => x.ServerId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_NetworkAdapterServers_ServerId",
                "NetworkAdapterServers",
                "ServerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "NetworkAdapterServers");
        }
    }
}