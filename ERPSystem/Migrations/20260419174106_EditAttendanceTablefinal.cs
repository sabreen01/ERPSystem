using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditAttendanceTablefinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "AttendanceManagment");

            migrationBuilder.RenameTable(
                name: "Attendance",
                schema: "Attendance",
                newName: "Attendance",
                newSchema: "AttendanceManagment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Attendance");

            migrationBuilder.RenameTable(
                name: "Attendance",
                schema: "AttendanceManagment",
                newName: "Attendance",
                newSchema: "Attendance");
        }
    }
}
