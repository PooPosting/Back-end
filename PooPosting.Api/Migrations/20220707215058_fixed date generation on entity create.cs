using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PooPosting.Api.Migrations
{
    public partial class fixeddategenerationonentitycreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 6, 18, 11, 25, 663, DateTimeKind.Local).AddTicks(1311));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CommentAdded",
                table: "Comments",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 6, 18, 11, 25, 662, DateTimeKind.Local).AddTicks(9572));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 7, 6, 18, 11, 25, 662, DateTimeKind.Local).AddTicks(6821));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PictureAdded",
                table: "Pictures",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 6, 18, 11, 25, 663, DateTimeKind.Local).AddTicks(1311),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CommentAdded",
                table: "Comments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 6, 18, 11, 25, 662, DateTimeKind.Local).AddTicks(9572),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreated",
                table: "Accounts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 6, 18, 11, 25, 662, DateTimeKind.Local).AddTicks(6821),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "now()");
        }
    }
}
