using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evidence.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrimeScenes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    DateReported = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrimeScenes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Evidences",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    CollectedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CollectedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CollecationLocation = table.Column<string>(type: "TEXT", nullable: false),
                    PhysicalDescription = table.Column<string>(type: "TEXT", nullable: false),
                    StorageLocation = table.Column<string>(type: "TEXT", nullable: false),
                    StorageRequirements = table.Column<string>(type: "TEXT", nullable: false),
                    HazmatStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    BiohazardStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    DispositionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DispositionMethod = table.Column<string>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<string>(type: "TEXT", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: true),
                    CreatedById = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastEditedById = table.Column<string>(type: "TEXT", nullable: true),
                    LastEditedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrimeSceneEvidences",
                columns: table => new
                {
                    CrimeSceneId = table.Column<string>(type: "TEXT", nullable: false),
                    EvidenceItemId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrimeSceneEvidences", x => new { x.CrimeSceneId, x.EvidenceItemId });
                    table.ForeignKey(
                        name: "FK_CrimeSceneEvidences_CrimeScenes_CrimeSceneId",
                        column: x => x.CrimeSceneId,
                        principalTable: "CrimeScenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrimeSceneEvidences_Evidences_EvidenceItemId",
                        column: x => x.EvidenceItemId,
                        principalTable: "Evidences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustodyLogs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    EvidenceItemId = table.Column<string>(type: "TEXT", nullable: false),
                    TransferredFromName = table.Column<string>(type: "TEXT", nullable: false),
                    TransferredToName = table.Column<string>(type: "TEXT", nullable: false),
                    TransferredAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Purpose = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Remarks = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustodyLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustodyLogs_Evidences_EvidenceItemId",
                        column: x => x.EvidenceItemId,
                        principalTable: "Evidences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabResults",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    EvidenceItemId = table.Column<string>(type: "TEXT", nullable: false),
                    TestName = table.Column<string>(type: "TEXT", nullable: false),
                    TestDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TestedBy = table.Column<string>(type: "TEXT", nullable: false),
                    Findings = table.Column<string>(type: "TEXT", nullable: false),
                    Conclusions = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabResults_Evidences_EvidenceItemId",
                        column: x => x.EvidenceItemId,
                        principalTable: "Evidences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    EvidenceItemId = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Evidences_EvidenceItemId",
                        column: x => x.EvidenceItemId,
                        principalTable: "Evidences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", nullable: false),
                    FileExtension = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    TakenAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TakenBy = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    EvidenceItemId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Evidences_EvidenceItemId",
                        column: x => x.EvidenceItemId,
                        principalTable: "Evidences",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CrimeScenePhotos",
                columns: table => new
                {
                    CrimeSceneId = table.Column<string>(type: "TEXT", nullable: false),
                    PhotoId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrimeScenePhotos", x => new { x.CrimeSceneId, x.PhotoId });
                    table.ForeignKey(
                        name: "FK_CrimeScenePhotos_CrimeScenes_CrimeSceneId",
                        column: x => x.CrimeSceneId,
                        principalTable: "CrimeScenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrimeScenePhotos_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrimeSceneEvidences_EvidenceItemId",
                table: "CrimeSceneEvidences",
                column: "EvidenceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CrimeScenePhotos_PhotoId",
                table: "CrimeScenePhotos",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_CustodyLogs_EvidenceItemId",
                table: "CustodyLogs",
                column: "EvidenceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_EvidenceItemId",
                table: "LabResults",
                column: "EvidenceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_EvidenceItemId",
                table: "Notes",
                column: "EvidenceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EvidenceItemId",
                table: "Photos",
                column: "EvidenceItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrimeSceneEvidences");

            migrationBuilder.DropTable(
                name: "CrimeScenePhotos");

            migrationBuilder.DropTable(
                name: "CustodyLogs");

            migrationBuilder.DropTable(
                name: "LabResults");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "CrimeScenes");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Evidences");
        }
    }
}
