using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class r1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enterAndExitList");

            migrationBuilder.DropTable(
                name: "vacationsList");

            migrationBuilder.CreateTable(
                name: "reportsList",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    ClockInTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ClockOutTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    TypeOfVacation = table.Column<int>(type: "int", nullable: false),
                    startDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndtDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reportsList", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_reportsList_empList_EmpId",
                        column: x => x.EmpId,
                        principalTable: "empList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reportsList_EmpId",
                table: "reportsList",
                column: "EmpId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reportsList");

            migrationBuilder.CreateTable(
                name: "enterAndExitList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClockInDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ClockInTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ClockOutTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enterAndExitList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vacationsList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndtDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TypeOfVacation = table.Column<int>(type: "int", nullable: false),
                    startDate = table.Column<DateOnly>(type: "date", nullable: false),
                    sumOdDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vacationsList", x => x.Id);
                });
        }
    }
}
