using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerSightAPI.Migrations
{
    public partial class AllModelsCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiKeys_AspNetUsers_OwnedById",
                table: "ApiKeys");

            migrationBuilder.DropForeignKey(
                name: "FK_CpuUsageServers_Servers_ServerId",
                table: "CpuUsageServers");

            migrationBuilder.DropForeignKey(
                name: "FK_HardDiskServers_Servers_ServerId",
                table: "HardDiskServers");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkAdapterServers_Servers_ServerId",
                table: "NetworkAdapterServers");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkUsage_Servers_ServerId",
                table: "NetworkUsage");

            migrationBuilder.DropForeignKey(
                name: "FK_PortServers_Servers_ServerId",
                table: "PortServers");

            migrationBuilder.DropForeignKey(
                name: "FK_RamUsages_Servers_ServerId",
                table: "RamUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_ServerEvents_Servers_ServerId",
                table: "ServerEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Servers_AspNetUsers_OwnedById",
                table: "Servers");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiKeys_AspNetUsers_OwnedById",
                table: "ApiKeys",
                column: "OwnedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CpuUsageServers_Servers_ServerId",
                table: "CpuUsageServers",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HardDiskServers_Servers_ServerId",
                table: "HardDiskServers",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkAdapterServers_Servers_ServerId",
                table: "NetworkAdapterServers",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkUsage_Servers_ServerId",
                table: "NetworkUsage",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PortServers_Servers_ServerId",
                table: "PortServers",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RamUsages_Servers_ServerId",
                table: "RamUsages",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServerEvents_Servers_ServerId",
                table: "ServerEvents",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Servers_AspNetUsers_OwnedById",
                table: "Servers",
                column: "OwnedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiKeys_AspNetUsers_OwnedById",
                table: "ApiKeys");

            migrationBuilder.DropForeignKey(
                name: "FK_CpuUsageServers_Servers_ServerId",
                table: "CpuUsageServers");

            migrationBuilder.DropForeignKey(
                name: "FK_HardDiskServers_Servers_ServerId",
                table: "HardDiskServers");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkAdapterServers_Servers_ServerId",
                table: "NetworkAdapterServers");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkUsage_Servers_ServerId",
                table: "NetworkUsage");

            migrationBuilder.DropForeignKey(
                name: "FK_PortServers_Servers_ServerId",
                table: "PortServers");

            migrationBuilder.DropForeignKey(
                name: "FK_RamUsages_Servers_ServerId",
                table: "RamUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_ServerEvents_Servers_ServerId",
                table: "ServerEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Servers_AspNetUsers_OwnedById",
                table: "Servers");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiKeys_AspNetUsers_OwnedById",
                table: "ApiKeys",
                column: "OwnedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpuUsageServers_Servers_ServerId",
                table: "CpuUsageServers",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HardDiskServers_Servers_ServerId",
                table: "HardDiskServers",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkAdapterServers_Servers_ServerId",
                table: "NetworkAdapterServers",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkUsage_Servers_ServerId",
                table: "NetworkUsage",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PortServers_Servers_ServerId",
                table: "PortServers",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RamUsages_Servers_ServerId",
                table: "RamUsages",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServerEvents_Servers_ServerId",
                table: "ServerEvents",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Servers_AspNetUsers_OwnedById",
                table: "Servers",
                column: "OwnedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
