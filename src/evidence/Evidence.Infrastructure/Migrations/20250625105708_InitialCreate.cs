using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evidence.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Evidences",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    S3Key = table.Column<string>(type: "text", nullable: false),
                    BucketName = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CollectionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UploadedById = table.Column<string>(type: "text", nullable: false),
                    UploadedByUsername = table.Column<string>(type: "text", nullable: false),
                    UploadedByEmail = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EvidenceTags",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    EvidenceId = table.Column<string>(type: "text", nullable: false),
                    TagId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvidenceTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvidenceTags_Evidences_EvidenceId",
                        column: x => x.EvidenceId,
                        principalTable: "Evidences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvidenceTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_Id",
                table: "Evidences",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_ReferenceNumber",
                table: "Evidences",
                column: "ReferenceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceTags_EvidenceId",
                table: "EvidenceTags",
                column: "EvidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceTags_TagId",
                table: "EvidenceTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Id",
                table: "Tags",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvidenceTags");

            migrationBuilder.DropTable(
                name: "Evidences");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
