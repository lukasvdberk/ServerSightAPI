using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerSightAPI.Migrations
{
    public partial class AddApiKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "ApiKeys",
                table => new
                {
                    Key = table.Column<string>("text", nullable: true),
                    OwnedById = table.Column<string>("text", nullable: true),
                    CreatedAt = table.Column<DateTime>("timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        "FK_ApiKeys_AspNetUsers_OwnedById",
                        x => x.OwnedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_ApiKeys_OwnedById",
                "ApiKeys",
                "OwnedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "ApiKeys");
        }
    }
}