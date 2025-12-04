using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameMatch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSportsToGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sports",
                table: "Groups",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sports",
                table: "Groups");
        }
    }
}
