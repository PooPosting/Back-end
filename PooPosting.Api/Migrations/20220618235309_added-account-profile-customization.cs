using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PooPosting.Api.Migrations
{
    public partial class addedaccountprofilecustomization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundPicUrl",
                table: "Accounts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicUrl",
                table: "Accounts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Verified",
                table: "Accounts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundPicUrl",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ProfilePicUrl",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Verified",
                table: "Accounts");
        }
    }
}
