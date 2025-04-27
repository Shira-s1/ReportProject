using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class c1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reportsList_empList_EmpId",
                table: "reportsList");

            migrationBuilder.RenameColumn(
                name: "EmpId",
                table: "reportsList",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_reportsList_EmpId",
                table: "reportsList",
                newName: "IX_reportsList_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_reportsList_empList_EmployeeId",
                table: "reportsList",
                column: "EmployeeId",
                principalTable: "empList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reportsList_empList_EmployeeId",
                table: "reportsList");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "reportsList",
                newName: "EmpId");

            migrationBuilder.RenameIndex(
                name: "IX_reportsList_EmployeeId",
                table: "reportsList",
                newName: "IX_reportsList_EmpId");

            migrationBuilder.AddForeignKey(
                name: "FK_reportsList_empList_EmpId",
                table: "reportsList",
                column: "EmpId",
                principalTable: "empList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
