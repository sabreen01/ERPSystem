using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSystem.Migrations
{
    /// <inheritdoc />
    public partial class EditAttendanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Employee_EmployeeId",
                table: "Attendances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attendances",
                table: "Attendances");

            migrationBuilder.EnsureSchema(
                name: "Attendance");

            migrationBuilder.RenameTable(
                name: "Attendances",
                newName: "Attendance",
                newSchema: "Attendance");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_EmployeeId",
                schema: "Attendance",
                table: "Attendance",
                newName: "IX_Attendance_EmployeeId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                schema: "Attendance",
                table: "Attendance",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Attendance",
                table: "Attendance",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OvertimeHours",
                schema: "Attendance",
                table: "Attendance",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "Attendance",
                table: "Attendance",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attendance",
                schema: "Attendance",
                table: "Attendance",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Employee_EmployeeId",
                schema: "Attendance",
                table: "Attendance",
                column: "EmployeeId",
                principalSchema: "HR",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Employee_EmployeeId",
                schema: "Attendance",
                table: "Attendance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attendance",
                schema: "Attendance",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "Date",
                schema: "Attendance",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Attendance",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "OvertimeHours",
                schema: "Attendance",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Attendance",
                table: "Attendance");

            migrationBuilder.RenameTable(
                name: "Attendance",
                schema: "Attendance",
                newName: "Attendances");

            migrationBuilder.RenameIndex(
                name: "IX_Attendance_EmployeeId",
                table: "Attendances",
                newName: "IX_Attendances_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attendances",
                table: "Attendances",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Employee_EmployeeId",
                table: "Attendances",
                column: "EmployeeId",
                principalSchema: "HR",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
