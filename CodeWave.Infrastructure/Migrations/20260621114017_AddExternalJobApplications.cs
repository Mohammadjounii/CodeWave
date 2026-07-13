using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeWave.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalJobApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalJobApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalJobId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    EmployerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmployerLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplyLink = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    MatchPercentage = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    AppliedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalJobApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalJobApplications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalJobApplications_UserId",
                table: "ExternalJobApplications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalJobApplications");
        }
    }
}
