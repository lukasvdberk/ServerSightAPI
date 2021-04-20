using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerSightAPI.Migrations
{
    public partial class NetworkAdaptersToservermodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_NetworkAdapterServers_AspNetUsers_ServerId",
                "NetworkAdapterServers");

            migrationBuilder.AddForeignKey(
                "FK_NetworkAdapterServers_Servers_ServerId",
                "NetworkAdapterServers",
                "ServerId",
                "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_NetworkAdapterServers_Servers_ServerId",
                "NetworkAdapterServers");

            migrationBuilder.AddForeignKey(
                "FK_NetworkAdapterServers_AspNetUsers_ServerId",
                "NetworkAdapterServers",
                "ServerId",
                "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}