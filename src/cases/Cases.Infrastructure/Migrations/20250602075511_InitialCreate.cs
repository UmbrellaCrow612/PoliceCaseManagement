using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cases.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CaseNumber = table.Column<string>(type: "text", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IncidentDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReportedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    ReportingOfficerId = table.Column<string>(type: "text", nullable: false),
                    ReportingOfficerUserName = table.Column<string>(type: "text", nullable: false),
                    ReportingOfficerEmail = table.Column<string>(type: "text", nullable: false),
                    CreatedById = table.Column<string>(type: "text", nullable: false),
                    CreatedByUserName = table.Column<string>(type: "text", nullable: false),
                    CreatedByEmail = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncidentTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaseActions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<string>(type: "text", nullable: false),
                    CreatedByName = table.Column<string>(type: "text", nullable: false),
                    CreatedByEmail = table.Column<string>(type: "text", nullable: false),
                    CaseId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseActions_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseAttachmentFiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    S3Key = table.Column<string>(type: "text", nullable: false),
                    BucketName = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CaseId = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseAttachmentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseAttachmentFiles_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CasePermissions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CanEdit = table.Column<bool>(type: "boolean", nullable: false),
                    CanViewPermissions = table.Column<bool>(type: "boolean", nullable: false),
                    CanEditPermissions = table.Column<bool>(type: "boolean", nullable: false),
                    CanViewFileAttachments = table.Column<bool>(type: "boolean", nullable: false),
                    CanDeleteFileAttachments = table.Column<bool>(type: "boolean", nullable: false),
                    CanViewAssigned = table.Column<bool>(type: "boolean", nullable: false),
                    CanAssign = table.Column<bool>(type: "boolean", nullable: false),
                    CanRemoveAssigned = table.Column<bool>(type: "boolean", nullable: false),
                    CanViewActions = table.Column<bool>(type: "boolean", nullable: false),
                    CanAddActions = table.Column<bool>(type: "boolean", nullable: false),
                    CanEditActions = table.Column<bool>(type: "boolean", nullable: false),
                    CanDeleteActions = table.Column<bool>(type: "boolean", nullable: false),
                    CaseId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CasePermissions_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    UserEmail = table.Column<string>(type: "text", nullable: false),
                    CaseId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseUsers_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseIncidentTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CaseId = table.Column<string>(type: "text", nullable: false),
                    IncidentTypeId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseIncidentTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseIncidentTypes_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseIncidentTypes_IncidentTypes_IncidentTypeId",
                        column: x => x.IncidentTypeId,
                        principalTable: "IncidentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseActions_CaseId",
                table: "CaseActions",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseActions_CreatedById",
                table: "CaseActions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CaseActions_Id",
                table: "CaseActions",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseAttachmentFiles_CaseId",
                table: "CaseAttachmentFiles",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseAttachmentFiles_Id",
                table: "CaseAttachmentFiles",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseIncidentTypes_CaseId",
                table: "CaseIncidentTypes",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseIncidentTypes_IncidentTypeId",
                table: "CaseIncidentTypes",
                column: "IncidentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CasePermissions_CaseId",
                table: "CasePermissions",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CasePermissions_Id",
                table: "CasePermissions",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CasePermissions_UserId",
                table: "CasePermissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_CaseNumber",
                table: "Cases",
                column: "CaseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cases_Id",
                table: "Cases",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ReportingOfficerId",
                table: "Cases",
                column: "ReportingOfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseUsers_CaseId",
                table: "CaseUsers",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseUsers_Id",
                table: "CaseUsers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseUsers_UserId",
                table: "CaseUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseActions");

            migrationBuilder.DropTable(
                name: "CaseAttachmentFiles");

            migrationBuilder.DropTable(
                name: "CaseIncidentTypes");

            migrationBuilder.DropTable(
                name: "CasePermissions");

            migrationBuilder.DropTable(
                name: "CaseUsers");

            migrationBuilder.DropTable(
                name: "IncidentTypes");

            migrationBuilder.DropTable(
                name: "Cases");
        }
    }
}
