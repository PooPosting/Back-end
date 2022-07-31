using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PicturesAPI.Migrations
{
    public partial class lastmigrationcleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PictureAccountJoin_Accounts_PictureId",
                table: "PictureAccountJoin");

            migrationBuilder.DropForeignKey(
                name: "FK_PictureAccountJoin_Pictures_AccountId",
                table: "PictureAccountJoin");

            migrationBuilder.DropForeignKey(
                name: "FK_PictureTagJoins_Tag_TagId",
                table: "PictureTagJoins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PictureAccountJoin",
                table: "PictureAccountJoin");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "PictureAccountJoin",
                newName: "PictureAccountJoins");

            migrationBuilder.RenameIndex(
                name: "IX_PictureAccountJoin_PictureId",
                table: "PictureAccountJoins",
                newName: "IX_PictureAccountJoins_PictureId");

            migrationBuilder.RenameIndex(
                name: "IX_PictureAccountJoin_AccountId",
                table: "PictureAccountJoins",
                newName: "IX_PictureAccountJoins_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PictureTagJoins_Tags_TagId",
                table: "PictureTagJoins",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PictureAccountJoins_Accounts_PictureId",
                table: "PictureAccountJoins");

            migrationBuilder.DropForeignKey(
                name: "FK_PictureAccountJoins_Pictures_AccountId",
                table: "PictureAccountJoins");

            migrationBuilder.DropForeignKey(
                name: "FK_PictureTagJoins_Tags_TagId",
                table: "PictureTagJoins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PictureAccountJoins",
                table: "PictureAccountJoins");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.RenameTable(
                name: "PictureAccountJoins",
                newName: "PictureAccountJoin");

            migrationBuilder.RenameIndex(
                name: "IX_PictureAccountJoins_PictureId",
                table: "PictureAccountJoin",
                newName: "IX_PictureAccountJoin_PictureId");

            migrationBuilder.RenameIndex(
                name: "IX_PictureAccountJoins_AccountId",
                table: "PictureAccountJoin",
                newName: "IX_PictureAccountJoin_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PictureAccountJoin",
                table: "PictureAccountJoin",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PictureAccountJoin_Accounts_PictureId",
                table: "PictureAccountJoin",
                column: "PictureId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PictureAccountJoin_Pictures_AccountId",
                table: "PictureAccountJoin",
                column: "AccountId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PictureTagJoins_Tag_TagId",
                table: "PictureTagJoins",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
