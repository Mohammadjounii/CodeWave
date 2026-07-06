using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeWave.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAgeFromCV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "CVs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "CVs",
                type: "int",
                nullable: true);
        }
    }
}
