using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PooPosting.Api.Migrations
{
    public partial class addedkeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Accounts_AccountId",
                table: "Pictures");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 12, 29, 45, 106, DateTimeKind.Local).AddTicks(9385),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 11, 57, 20, 87, DateTimeKind.Local).AddTicks(8138));

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Pictures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CommentAdded",
                table: "Comments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 12, 29, 45, 106, DateTimeKind.Local).AddTicks(7003),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 11, 57, 20, 87, DateTimeKind.Local).AddTicks(7173));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 12, 29, 45, 106, DateTimeKind.Local).AddTicks(5115),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 11, 57, 20, 87, DateTimeKind.Local).AddTicks(6321));

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Accounts_AccountId",
                table: "Pictures",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Accounts_AccountId",
                table: "Pictures");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 11, 57, 20, 87, DateTimeKind.Local).AddTicks(8138),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 12, 29, 45, 106, DateTimeKind.Local).AddTicks(9385));

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Pictures",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CommentAdded",
                table: "Comments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 11, 57, 20, 87, DateTimeKind.Local).AddTicks(7173),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 12, 29, 45, 106, DateTimeKind.Local).AddTicks(7003));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 11, 57, 20, 87, DateTimeKind.Local).AddTicks(6321),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 12, 29, 45, 106, DateTimeKind.Local).AddTicks(5115));

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Accounts_AccountId",
                table: "Pictures",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
