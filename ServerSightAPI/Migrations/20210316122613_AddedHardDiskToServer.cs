using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerSightAPI.Migrations
{
    public partial class AddedHardDiskToServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "HardDiskServers",
                table => new
                {
                    Id = table.Column<string>("text", nullable: false),
                    DiskName = table.Column<string>("text", nullable: true),
                    SpaceAvailable = table.Column<float>("real", nullable: false),
                    SpaceTotal = table.Column<float>("real", nullable: false),
                    ServerId = table.Column<string>("text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HardDiskServers", x => x.Id);
                    table.ForeignKey(
                        "FK_HardDiskServers_Servers_ServerId",
                        x => x.ServerId,
                        "Servers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_HardDiskServers_ServerId",
                "HardDiskServers",
                "ServerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "HardDiskServers");
        }
    }
}