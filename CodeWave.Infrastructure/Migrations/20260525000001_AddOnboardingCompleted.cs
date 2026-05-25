using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeWave.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOnboardingCompleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OnboardingCompleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Backfill: users who already finished the full flow (have a LearningPath) are considered complete
            migrationBuilder.Sql(
                "UPDATE AspNetUsers SET OnboardingCompleted = 1 WHERE LearningPath IS NOT NULL AND LearningPath != ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnboardingCompleted",
                table: "AspNetUsers");
        }
    }
}
