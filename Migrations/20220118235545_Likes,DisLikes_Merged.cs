using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PicturesAPI.Migrations
{
    public partial class LikesDisLikes_Merged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dislikes_Accounts_DisLikerId",
                table: "Dislikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Dislikes_Pictures_DisLikedId",
                table: "Dislikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dislikes",
                table: "Dislikes");

            migrationBuilder.RenameTable(
                name: "Dislikes",
                newName: "Dislike");

            migrationBuilder.RenameIndex(
                name: "IX_Dislikes_DisLikerId",
                table: "Dislike",
                newName: "IX_Dislike_DisLikerId");

            migrationBuilder.RenameIndex(
                name: "IX_Dislikes_DisLikedId",
                table: "Dislike",
                newName: "IX_Dislike_DisLikedId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 1, 13, 9, 21, 17, 685, DateTimeKind.Local).AddTicks(9943));

            migrationBuilder.AddColumn<bool>(
                name: "IsLike",
                table: "Likes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 1, 13, 9, 21, 17, 691, DateTimeKind.Local).AddTicks(6445));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dislike",
                table: "Dislike",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dislike_Accounts_DisLikerId",
                table: "Dislike",
                column: "DisLikerId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dislike_Pictures_DisLikedId",
                table: "Dislike",
                column: "DisLikedId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dislike_Accounts_DisLikerId",
                table: "Dislike");

            migrationBuilder.DropForeignKey(
                name: "FK_Dislike_Pictures_DisLikedId",
                table: "Dislike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dislike",
                table: "Dislike");

            migrationBuilder.DropColumn(
                name: "IsLike",
                table: "Likes");

            migrationBuilder.RenameTable(
                name: "Dislike",
                newName: "Dislikes");

            migrationBuilder.RenameIndex(
                name: "IX_Dislike_DisLikerId",
                table: "Dislikes",
                newName: "IX_Dislikes_DisLikerId");

            migrationBuilder.RenameIndex(
                name: "IX_Dislike_DisLikedId",
                table: "Dislikes",
                newName: "IX_Dislikes_DisLikedId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 13, 9, 21, 17, 685, DateTimeKind.Local).AddTicks(9943),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 13, 9, 21, 17, 691, DateTimeKind.Local).AddTicks(6445),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dislikes",
                table: "Dislikes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dislikes_Accounts_DisLikerId",
                table: "Dislikes",
                column: "DisLikerId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dislikes_Pictures_DisLikedId",
                table: "Dislikes",
                column: "DisLikedId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
