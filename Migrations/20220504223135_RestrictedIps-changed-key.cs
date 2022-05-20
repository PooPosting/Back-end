using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PicturesAPI.Migrations
{
    public partial class RestrictedIpschangedkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RestrictedIps",
                table: "RestrictedIps");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RestrictedIps");

            migrationBuilder.DropColumn(
                name: "CantComment",
                table: "RestrictedIps");

            migrationBuilder.UpdateData(
                table: "RestrictedIps",
                keyColumn: "IpAddress",
                keyValue: null,
                column: "IpAddress",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "RestrictedIps",
                type: "varchar(95)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RestrictedIps",
                table: "RestrictedIps",
                column: "IpAddress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RestrictedIps",
                table: "RestrictedIps");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "RestrictedIps",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(95)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "RestrictedIps",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<bool>(
                name: "CantComment",
                table: "RestrictedIps",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RestrictedIps",
                table: "RestrictedIps",
                column: "Id");
        }
    }
}
