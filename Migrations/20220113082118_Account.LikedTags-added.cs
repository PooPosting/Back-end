using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PicturesAPI.Migrations
{
    public partial class AccountLikedTagsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 13, 9, 21, 17, 685, DateTimeKind.Local).AddTicks(9943),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 1, 6, 13, 0, 25, 848, DateTimeKind.Local).AddTicks(114));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 13, 9, 21, 17, 691, DateTimeKind.Local).AddTicks(6445),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 1, 6, 13, 0, 25, 857, DateTimeKind.Local).AddTicks(1096));

            migrationBuilder.AddColumn<string>(
                name: "LikedTags",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikedTags",
                table: "Accounts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 6, 13, 0, 25, 848, DateTimeKind.Local).AddTicks(114),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 1, 13, 9, 21, 17, 685, DateTimeKind.Local).AddTicks(9943));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 6, 13, 0, 25, 857, DateTimeKind.Local).AddTicks(1096),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 1, 13, 9, 21, 17, 691, DateTimeKind.Local).AddTicks(6445));
        }
    }
}
