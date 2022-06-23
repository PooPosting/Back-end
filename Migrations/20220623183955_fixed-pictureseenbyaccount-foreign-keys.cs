using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PicturesAPI.Migrations
{
    public partial class fixedpictureseenbyaccountforeignkeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PictureAccountJoins_Accounts_PictureId",
                table: "PictureAccountJoins");

            migrationBuilder.DropForeignKey(
                name: "FK_PictureAccountJoins_Pictures_AccountId",
                table: "PictureAccountJoins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PictureAccountJoins",
                table: "PictureAccountJoins");

            migrationBuilder.RenameTable(
                name: "PictureAccountJoins",
                newName: "PictureSeenByAccountJoins");

            migrationBuilder.RenameIndex(
                name: "IX_PictureAccountJoins_PictureId",
                table: "PictureSeenByAccountJoins",
                newName: "IX_PictureSeenByAccountJoins_PictureId");

            migrationBuilder.RenameIndex(
                name: "IX_PictureAccountJoins_AccountId",
                table: "PictureSeenByAccountJoins",
                newName: "IX_PictureSeenByAccountJoins_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PictureSeenByAccountJoins",
                table: "PictureSeenByAccountJoins",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PictureSeenByAccountJoins_Accounts_AccountId",
                table: "PictureSeenByAccountJoins",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PictureSeenByAccountJoins_Pictures_PictureId",
                table: "PictureSeenByAccountJoins",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PictureSeenByAccountJoins_Accounts_AccountId",
                table: "PictureSeenByAccountJoins");

            migrationBuilder.DropForeignKey(
                name: "FK_PictureSeenByAccountJoins_Pictures_PictureId",
                table: "PictureSeenByAccountJoins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PictureSeenByAccountJoins",
                table: "PictureSeenByAccountJoins");

            migrationBuilder.RenameTable(
                name: "PictureSeenByAccountJoins",
                newName: "PictureAccountJoins");

            migrationBuilder.RenameIndex(
                name: "IX_PictureSeenByAccountJoins_PictureId",
                table: "PictureAccountJoins",
                newName: "IX_PictureAccountJoins_PictureId");

            migrationBuilder.RenameIndex(
                name: "IX_PictureSeenByAccountJoins_AccountId",
                table: "PictureAccountJoins",
                newName: "IX_PictureAccountJoins_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PictureAccountJoins",
                table: "PictureAccountJoins",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PictureAccountJoins_Accounts_PictureId",
                table: "PictureAccountJoins",
                column: "PictureId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PictureAccountJoins_Pictures_AccountId",
                table: "PictureAccountJoins",
                column: "AccountId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
