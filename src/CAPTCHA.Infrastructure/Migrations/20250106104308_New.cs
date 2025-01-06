using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAPTCHA.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CAPTCHAMathQuestions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    AnswerHash = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "INTEGER", nullable: false),
                    SuccessfulAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IPAddress = table.Column<string>(type: "TEXT", nullable: true),
                    Attempts = table.Column<int>(type: "INTEGER", nullable: false),
                    UserAgent = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAPTCHAMathQuestions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CAPTCHAMathQuestions");
        }
    }
}
