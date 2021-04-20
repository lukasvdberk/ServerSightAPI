using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerSightAPI.Migrations
{
    public partial class ServerOwnerToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Servers",
                table => new
                {
                    Id = table.Column<string>("text", nullable: false),
                    Title = table.Column<string>("text", nullable: true),
                    OwnedById = table.Column<string>("text", nullable: true),
                    PowerStatus = table.Column<bool>("boolean", nullable: false),
                    Description = table.Column<string>("text", nullable: true),
                    CreatedAt = table.Column<DateTime>("timestamp", nullable: false),
                    ImagePath = table.Column<string>("text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                    table.ForeignKey(
                        "FK_Servers_AspNetUsers_OwnedById",
                        x => x.OwnedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_Servers_OwnedById",
                "Servers",
                "OwnedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Servers");
        }
    }
}