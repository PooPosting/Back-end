using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PooPosting.Api.Migrations
{
    public partial class attributenamestandarization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Accounts_AuthorId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Comments",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                newName: "IX_Comments_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Accounts_AccountId",
                table: "Comments",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Accounts_AccountId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Comments",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_AccountId",
                table: "Comments",
                newName: "IX_Comments_AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Accounts_AuthorId",
                table: "Comments",
                column: "AuthorId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
