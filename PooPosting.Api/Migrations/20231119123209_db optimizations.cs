using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PooPosting.Api.Migrations
{
    /// <inheritdoc />
    public partial class dboptimizations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tags_Value",
                table: "Tags",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_PictureAdded",
                table: "Pictures",
                column: "PictureAdded");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_Liked",
                table: "Likes",
                column: "Liked");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentAdded",
                table: "Comments",
                column: "CommentAdded");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_Value",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_PictureAdded",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Likes_Liked",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CommentAdded",
                table: "Comments");
        }
    }
}
