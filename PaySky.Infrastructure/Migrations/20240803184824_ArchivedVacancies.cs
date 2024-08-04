using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaySky.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ArchivedVacancies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ArchivedVacancyId",
                table: "Applications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArchivedVacancies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    MaxApplications = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivedVacancies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ArchivedVacancyId",
                table: "Applications",
                column: "ArchivedVacancyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_ArchivedVacancies_ArchivedVacancyId",
                table: "Applications",
                column: "ArchivedVacancyId",
                principalTable: "ArchivedVacancies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_ArchivedVacancies_ArchivedVacancyId",
                table: "Applications");

            migrationBuilder.DropTable(
                name: "ArchivedVacancies");

            migrationBuilder.DropIndex(
                name: "IX_Applications_ArchivedVacancyId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ArchivedVacancyId",
                table: "Applications");
        }
    }
}
