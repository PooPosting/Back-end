using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PicturesAPI.Migrations
{
    public partial class changedkeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountsLikedTags_Accounts_AccountId",
                table: "AccountsLikedTags");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountsLikedTags_Tags_TagId",
                table: "AccountsLikedTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Accounts_LikerId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Pictures_LikedId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_PicturesSeenByAccounts_Accounts_AccountId",
                table: "PicturesSeenByAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PicturesSeenByAccounts_Pictures_PictureId",
                table: "PicturesSeenByAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PictureTags_Pictures_PictureId",
                table: "PictureTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PictureTags_Tags_TagId",
                table: "PictureTags");

            migrationBuilder.DropIndex(
                name: "IX_Likes_LikedId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_LikerId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "LikedId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "LikerId",
                table: "Likes");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "PictureTags",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PictureId",
                table: "PictureTags",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PictureId",
                table: "PicturesSeenByAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "PicturesSeenByAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 15, 11, 26, 379, DateTimeKind.Local).AddTicks(788),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 12, 29, 45, 106, DateTimeKind.Local).AddTicks(9385));

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Likes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PictureId",
                table: "Likes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CommentAdded",
                table: "Comments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 15, 11, 26, 378, DateTimeKind.Local).AddTicks(9767),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 12, 29, 45, 106, DateTimeKind.Local).AddTicks(7003));

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "AccountsLikedTags",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "AccountsLikedTags",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 15, 11, 26, 378, DateTimeKind.Local).AddTicks(8997),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 12, 29, 45, 106, DateTimeKind.Local).AddTicks(5115));

            migrationBuilder.CreateIndex(
                name: "IX_Likes_AccountId",
                table: "Likes",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_PictureId",
                table: "Likes",
                column: "PictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountsLikedTags_Accounts_AccountId",
                table: "AccountsLikedTags",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountsLikedTags_Tags_TagId",
                table: "AccountsLikedTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Accounts_AccountId",
                table: "Likes",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Pictures_PictureId",
                table: "Likes",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PicturesSeenByAccounts_Accounts_AccountId",
                table: "PicturesSeenByAccounts",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PicturesSeenByAccounts_Pictures_PictureId",
                table: "PicturesSeenByAccounts",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PictureTags_Pictures_PictureId",
                table: "PictureTags",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PictureTags_Tags_TagId",
                table: "PictureTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountsLikedTags_Accounts_AccountId",
                table: "AccountsLikedTags");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountsLikedTags_Tags_TagId",
                table: "AccountsLikedTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Accounts_AccountId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Pictures_PictureId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_PicturesSeenByAccounts_Accounts_AccountId",
                table: "PicturesSeenByAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PicturesSeenByAccounts_Pictures_PictureId",
                table: "PicturesSeenByAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PictureTags_Pictures_PictureId",
                table: "PictureTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PictureTags_Tags_TagId",
                table: "PictureTags");

            migrationBuilder.DropIndex(
                name: "IX_Likes_AccountId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_PictureId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "Likes");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "PictureTags",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PictureId",
                table: "PictureTags",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PictureId",
                table: "PicturesSeenByAccounts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "PicturesSeenByAccounts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 12, 29, 45, 106, DateTimeKind.Local).AddTicks(9385),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 15, 11, 26, 379, DateTimeKind.Local).AddTicks(788));

            migrationBuilder.AddColumn<int>(
                name: "LikedId",
                table: "Likes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LikerId",
                table: "Likes",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CommentAdded",
                table: "Comments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 12, 29, 45, 106, DateTimeKind.Local).AddTicks(7003),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 15, 11, 26, 378, DateTimeKind.Local).AddTicks(9767));

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "AccountsLikedTags",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "AccountsLikedTags",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 12, 29, 45, 106, DateTimeKind.Local).AddTicks(5115),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 15, 11, 26, 378, DateTimeKind.Local).AddTicks(8997));

            migrationBuilder.CreateIndex(
                name: "IX_Likes_LikedId",
                table: "Likes",
                column: "LikedId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_LikerId",
                table: "Likes",
                column: "LikerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountsLikedTags_Accounts_AccountId",
                table: "AccountsLikedTags",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountsLikedTags_Tags_TagId",
                table: "AccountsLikedTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Accounts_LikerId",
                table: "Likes",
                column: "LikerId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Pictures_LikedId",
                table: "Likes",
                column: "LikedId",
                principalTable: "Pictures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PicturesSeenByAccounts_Accounts_AccountId",
                table: "PicturesSeenByAccounts",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PicturesSeenByAccounts_Pictures_PictureId",
                table: "PicturesSeenByAccounts",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PictureTags_Pictures_PictureId",
                table: "PictureTags",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PictureTags_Tags_TagId",
                table: "PictureTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");
        }
    }
}
