using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PooPosting.Api.Migrations
{
    /// <inheritdoc />
    public partial class updatedlike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLike",
                table: "Likes");

            migrationBuilder.AddColumn<DateTime>(
                name: "Liked",
                table: "Likes",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "now()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Liked",
                table: "Likes");

            migrationBuilder.AddColumn<bool>(
                name: "IsLike",
                table: "Likes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
