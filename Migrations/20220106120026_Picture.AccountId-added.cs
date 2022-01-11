using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PicturesAPI.Migrations
{
    public partial class PictureAccountIdadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 6, 13, 0, 25, 848, DateTimeKind.Local).AddTicks(114),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 1, 6, 2, 36, 55, 112, DateTimeKind.Local).AddTicks(3649));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 6, 13, 0, 25, 857, DateTimeKind.Local).AddTicks(1096),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 1, 6, 2, 36, 55, 118, DateTimeKind.Local).AddTicks(4296));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 6, 2, 36, 55, 112, DateTimeKind.Local).AddTicks(3649),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 1, 6, 13, 0, 25, 848, DateTimeKind.Local).AddTicks(114));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 6, 2, 36, 55, 118, DateTimeKind.Local).AddTicks(4296),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 1, 6, 13, 0, 25, 857, DateTimeKind.Local).AddTicks(1096));
        }
    }
}
