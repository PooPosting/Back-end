using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PooPosting.Api.Migrations
{
    public partial class entityperformanceupdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountLikedTagJoins_Accounts_AccountId",
                table: "AccountLikedTagJoins");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountLikedTagJoins_Tags_TagId",
                table: "AccountLikedTagJoins");

            migrationBuilder.DropForeignKey(
                name: "FK_PictureAccountJoins_Accounts_PictureId",
                table: "PictureAccountJoins");

            migrationBuilder.DropForeignKey(
                name: "FK_PictureAccountJoins_Pictures_AccountId",
                table: "PictureAccountJoins");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Accounts_AccountId",
                table: "Pictures");

            migrationBuilder.DropForeignKey(
                name: "FK_PictureTagJoins_Pictures_PictureId",
                table: "PictureTagJoins");

            migrationBuilder.DropForeignKey(
                name: "FK_PictureTagJoins_Tags_TagId",
                table: "PictureTagJoins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PictureTagJoins",
                table: "PictureTagJoins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PictureAccountJoins",
                table: "PictureAccountJoins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountLikedTagJoins",
                table: "AccountLikedTagJoins");

            migrationBuilder.RenameTable(
                name: "PictureTagJoins",
                newName: "PictureTags");

            migrationBuilder.RenameTable(
                name: "PictureAccountJoins",
                newName: "PicturesSeenByAccounts");

            migrationBuilder.RenameTable(
                name: "AccountLikedTagJoins",
                newName: "AccountsLikedTags");

            migrationBuilder.RenameIndex(
                name: "IX_PictureTagJoins_TagId",
                table: "PictureTags",
                newName: "IX_PictureTags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_PictureTagJoins_PictureId",
                table: "PictureTags",
                newName: "IX_PictureTags_PictureId");

            migrationBuilder.RenameIndex(
                name: "IX_PictureAccountJoins_PictureId",
                table: "PicturesSeenByAccounts",
                newName: "IX_PicturesSeenByAccounts_PictureId");

            migrationBuilder.RenameIndex(
                name: "IX_PictureAccountJoins_AccountId",
                table: "PicturesSeenByAccounts",
                newName: "IX_PicturesSeenByAccounts_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountLikedTagJoins_TagId",
                table: "AccountsLikedTags",
                newName: "IX_AccountsLikedTags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountLikedTagJoins_AccountId",
                table: "AccountsLikedTags",
                newName: "IX_AccountsLikedTags_AccountId");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Name",
                keyValue: null,
                column: "Name",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "varchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16)",
                oldMaxLength: 16,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "RestrictedIps",
                keyColumn: "IpAddress",
                keyValue: null,
                column: "IpAddress",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "RestrictedIps",
                type: "varchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "PopularityScore",
                table: "Pictures",
                type: "bigint",
                nullable: false,
                defaultValue: 36500L,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 11, 54, 4, 10, DateTimeKind.Local).AddTicks(2412),
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Pictures",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Pictures",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Comments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CommentAdded",
                table: "Comments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 11, 54, 4, 10, DateTimeKind.Local).AddTicks(1376),
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<bool>(
                name: "Verified",
                table: "Accounts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicUrl",
                table: "Accounts",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Accounts",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Accounts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Accounts",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldMaxLength: 40)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BackgroundPicUrl",
                table: "Accounts",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 11, 54, 4, 10, DateTimeKind.Local).AddTicks(410),
                oldClrType: typeof(DateTime),
                oldType: "datetime");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_PictureTags",
                table: "PictureTags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PicturesSeenByAccounts",
                table: "PicturesSeenByAccounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountsLikedTags",
                table: "AccountsLikedTags",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_Name",
                table: "Pictures",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Nickname",
                table: "Accounts",
                column: "Nickname");

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
                name: "FK_Pictures_Accounts_AccountId",
                table: "Pictures",
                column: "AccountId",
                principalTable: "Accounts",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountsLikedTags_Accounts_AccountId",
                table: "AccountsLikedTags");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountsLikedTags_Tags_TagId",
                table: "AccountsLikedTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Accounts_AccountId",
                table: "Pictures");

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
                name: "IX_Pictures_Name",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_Nickname",
                table: "Accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PictureTags",
                table: "PictureTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PicturesSeenByAccounts",
                table: "PicturesSeenByAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountsLikedTags",
                table: "AccountsLikedTags");

            migrationBuilder.RenameTable(
                name: "PictureTags",
                newName: "PictureTagJoins");

            migrationBuilder.RenameTable(
                name: "PicturesSeenByAccounts",
                newName: "PictureAccountJoins");

            migrationBuilder.RenameTable(
                name: "AccountsLikedTags",
                newName: "AccountLikedTagJoins");

            migrationBuilder.RenameIndex(
                name: "IX_PictureTags_TagId",
                table: "PictureTagJoins",
                newName: "IX_PictureTagJoins_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_PictureTags_PictureId",
                table: "PictureTagJoins",
                newName: "IX_PictureTagJoins_PictureId");

            migrationBuilder.RenameIndex(
                name: "IX_PicturesSeenByAccounts_PictureId",
                table: "PictureAccountJoins",
                newName: "IX_PictureAccountJoins_PictureId");

            migrationBuilder.RenameIndex(
                name: "IX_PicturesSeenByAccounts_AccountId",
                table: "PictureAccountJoins",
                newName: "IX_PictureAccountJoins_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountsLikedTags_TagId",
                table: "AccountLikedTagJoins",
                newName: "IX_AccountLikedTagJoins_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountsLikedTags_AccountId",
                table: "AccountLikedTagJoins",
                newName: "IX_AccountLikedTagJoins_AccountId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "varchar(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(16)",
                oldMaxLength: 16)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "RestrictedIps",
                type: "varchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "PopularityScore",
                table: "Pictures",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 36500L);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 11, 54, 4, 10, DateTimeKind.Local).AddTicks(2412));

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Pictures",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Pictures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Comments",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CommentAdded",
                table: "Comments",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 11, 54, 4, 10, DateTimeKind.Local).AddTicks(1376));

            migrationBuilder.AlterColumn<bool>(
                name: "Verified",
                table: "Accounts",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Accounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicUrl",
                table: "Accounts",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Accounts",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Accounts",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Accounts",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BackgroundPicUrl",
                table: "Accounts",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 11, 54, 4, 10, DateTimeKind.Local).AddTicks(410));

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "PictureTagJoins",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PictureId",
                table: "PictureTagJoins",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PictureId",
                table: "PictureAccountJoins",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "PictureAccountJoins",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "AccountLikedTagJoins",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "AccountLikedTagJoins",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PictureTagJoins",
                table: "PictureTagJoins",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PictureAccountJoins",
                table: "PictureAccountJoins",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountLikedTagJoins",
                table: "AccountLikedTagJoins",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountLikedTagJoins_Accounts_AccountId",
                table: "AccountLikedTagJoins",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountLikedTagJoins_Tags_TagId",
                table: "AccountLikedTagJoins",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Pictures_Accounts_AccountId",
                table: "Pictures",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PictureTagJoins_Pictures_PictureId",
                table: "PictureTagJoins",
                column: "PictureId",
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
    }
}
