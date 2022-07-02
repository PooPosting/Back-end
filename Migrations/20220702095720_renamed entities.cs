using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PicturesAPI.Migrations
{
    public partial class renamedentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 11, 57, 20, 87, DateTimeKind.Local).AddTicks(8138),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 11, 54, 4, 10, DateTimeKind.Local).AddTicks(2412));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CommentAdded",
                table: "Comments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 11, 57, 20, 87, DateTimeKind.Local).AddTicks(7173),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 11, 54, 4, 10, DateTimeKind.Local).AddTicks(1376));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 11, 57, 20, 87, DateTimeKind.Local).AddTicks(6321),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 11, 54, 4, 10, DateTimeKind.Local).AddTicks(410));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 11, 54, 4, 10, DateTimeKind.Local).AddTicks(2412),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 11, 57, 20, 87, DateTimeKind.Local).AddTicks(8138));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CommentAdded",
                table: "Comments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 11, 54, 4, 10, DateTimeKind.Local).AddTicks(1376),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 11, 57, 20, 87, DateTimeKind.Local).AddTicks(7173));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 2, 11, 54, 4, 10, DateTimeKind.Local).AddTicks(410),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 2, 11, 57, 20, 87, DateTimeKind.Local).AddTicks(6321));
        }
    }
}
