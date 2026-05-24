using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeWave.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDuplicateUserQuizAttemptFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuizAttempts_AspNetUsers_ApplicationUserId",
                table: "UserQuizAttempts");

            migrationBuilder.DropIndex(
                name: "IX_UserQuizAttempts_ApplicationUserId",
                table: "UserQuizAttempts");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserQuizAttempts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "UserQuizAttempts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAttempts_ApplicationUserId",
                table: "UserQuizAttempts",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuizAttempts_AspNetUsers_ApplicationUserId",
                table: "UserQuizAttempts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
