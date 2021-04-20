using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerSightAPI.Migrations
{
    public partial class ApiKeysRemovedKeyless : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                "Id",
                "ApiKeys",
                "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                "PK_ApiKeys",
                "ApiKeys",
                "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                "PK_ApiKeys",
                "ApiKeys");

            migrationBuilder.AlterColumn<string>(
                "Id",
                "ApiKeys",
                "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}