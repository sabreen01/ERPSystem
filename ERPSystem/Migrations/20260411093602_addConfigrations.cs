using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSystem.Migrations
{
    /// <inheritdoc />
    public partial class addConfigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "Identity",
                table: "UserRole",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Identity",
                table: "UserRole",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "Identity",
                table: "UserRole",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "Identity",
                table: "User",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "Identity",
                table: "User",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "Identity",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "Identity",
                table: "User",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "Identity",
                table: "User",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Identity",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "Identity",
                table: "User",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                schema: "Identity",
                table: "Role",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Identity",
                table: "Role",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "Identity",
                table: "Role",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Identity",
                table: "Role",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "Identity",
                table: "Role",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_CreatedAt",
                schema: "Identity",
                table: "UserRole",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_IsDeleted",
                schema: "Identity",
                table: "UserRole",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "Identity",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId_RoleId",
                schema: "Identity",
                table: "UserRole",
                columns: new[] { "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedAt",
                schema: "Identity",
                table: "User",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "Identity",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_IsDeleted",
                schema: "Identity",
                table: "User",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                schema: "Identity",
                table: "User",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreatedAt",
                schema: "Identity",
                table: "Role",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Role_IsDeleted",
                schema: "Identity",
                table: "Role",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                schema: "Identity",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_RoleId",
                schema: "Identity",
                table: "UserRole",
                column: "RoleId",
                principalSchema: "Identity",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_User_UserId",
                schema: "Identity",
                table: "UserRole",
                column: "UserId",
                principalSchema: "Identity",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role_RoleId",
                schema: "Identity",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_User_UserId",
                schema: "Identity",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_CreatedAt",
                schema: "Identity",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_IsDeleted",
                schema: "Identity",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_RoleId",
                schema: "Identity",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_UserId_RoleId",
                schema: "Identity",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_User_CreatedAt",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_IsDeleted",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UserName",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Role_CreatedAt",
                schema: "Identity",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_IsDeleted",
                schema: "Identity",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_Name",
                schema: "Identity",
                table: "Role");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "Identity",
                table: "UserRole",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Identity",
                table: "UserRole",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "Identity",
                table: "UserRole",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "Identity",
                table: "User",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "Identity",
                table: "User",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "Identity",
                table: "User",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "Identity",
                table: "User",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "Identity",
                table: "User",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Identity",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "Identity",
                table: "User",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                schema: "Identity",
                table: "Role",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Identity",
                table: "Role",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "Identity",
                table: "Role",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Identity",
                table: "Role",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "Identity",
                table: "Role",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");
        }
    }
}
