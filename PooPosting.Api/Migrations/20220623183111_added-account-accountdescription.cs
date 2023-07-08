using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PooPosting.Api.Migrations
{
    public partial class addedaccountaccountdescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountDescription",
                table: "Accounts",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountDescription",
                table: "Accounts");
        }
    }
}
