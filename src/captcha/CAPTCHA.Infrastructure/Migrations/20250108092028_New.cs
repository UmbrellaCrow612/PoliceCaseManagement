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
                name: "CAPTCHAAudioQuestions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    AnswerInPlainText = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "INTEGER", nullable: false),
                    SuccessfulAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Attempts = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAPTCHAAudioQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CAPTCHAGridParentQuestions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsUsed = table.Column<bool>(type: "INTEGER", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAPTCHAGridParentQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CAPTCHAMathQuestions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Answer = table.Column<double>(type: "REAL", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "CAPTCHAGridChildren",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CAPTCHAGridParentQuestionId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAPTCHAGridChildren", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CAPTCHAGridChildren_CAPTCHAGridParentQuestions_CAPTCHAGridParentQuestionId",
                        column: x => x.CAPTCHAGridParentQuestionId,
                        principalTable: "CAPTCHAGridParentQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CAPTCHAGridChildren_CAPTCHAGridParentQuestionId",
                table: "CAPTCHAGridChildren",
                column: "CAPTCHAGridParentQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_CAPTCHAGridParentQuestions_Id",
                table: "CAPTCHAGridParentQuestions",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CAPTCHAAudioQuestions");

            migrationBuilder.DropTable(
                name: "CAPTCHAGridChildren");

            migrationBuilder.DropTable(
                name: "CAPTCHAMathQuestions");

            migrationBuilder.DropTable(
                name: "CAPTCHAGridParentQuestions");
        }
    }
}
