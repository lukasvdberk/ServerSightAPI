using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerSightAPI.Migrations
{
    public partial class NotificationsThresholdmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationTreshold",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    RamUsageThresholdInPercentage = table.Column<int>(type: "integer", nullable: false),
                    CpuUsageThresholdInPercentage = table.Column<int>(type: "integer", nullable: false),
                    HardDiskUsageThresholdInPercentage = table.Column<int>(type: "integer", nullable: false),
                    ServerId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTreshold", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTreshold_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTreshold_ServerId",
                table: "NotificationTreshold",
                column: "ServerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationTreshold");
        }
    }
}
