using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballRankingSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangesOnMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayedAt",
                table: "Matches",
                newName: "PlayedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayedOn",
                table: "Matches",
                newName: "PlayedAt");
        }
    }
}
