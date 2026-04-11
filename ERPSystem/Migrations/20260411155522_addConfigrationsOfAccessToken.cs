using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSystem.Migrations
{
    /// <inheritdoc />
    public partial class addConfigrationsOfAccessToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TokenId",
                schema: "Identity",
                table: "AccessToken",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "Identity",
                table: "AccessToken",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Identity",
                table: "AccessToken",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "Identity",
                table: "AccessToken",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_AccessToken_CreatedAt",
                schema: "Identity",
                table: "AccessToken",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AccessToken_IsDeleted",
                schema: "Identity",
                table: "AccessToken",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AccessToken_RoleId",
                schema: "Identity",
                table: "AccessToken",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessToken_UserId",
                schema: "Identity",
                table: "AccessToken",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessToken_Role_RoleId",
                schema: "Identity",
                table: "AccessToken",
                column: "RoleId",
                principalSchema: "Identity",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessToken_User_UserId",
                schema: "Identity",
                table: "AccessToken",
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
                name: "FK_AccessToken_Role_RoleId",
                schema: "Identity",
                table: "AccessToken");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessToken_User_UserId",
                schema: "Identity",
                table: "AccessToken");

            migrationBuilder.DropIndex(
                name: "IX_AccessToken_CreatedAt",
                schema: "Identity",
                table: "AccessToken");

            migrationBuilder.DropIndex(
                name: "IX_AccessToken_IsDeleted",
                schema: "Identity",
                table: "AccessToken");

            migrationBuilder.DropIndex(
                name: "IX_AccessToken_RoleId",
                schema: "Identity",
                table: "AccessToken");

            migrationBuilder.DropIndex(
                name: "IX_AccessToken_UserId",
                schema: "Identity",
                table: "AccessToken");

            migrationBuilder.AlterColumn<string>(
                name: "TokenId",
                schema: "Identity",
                table: "AccessToken",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "Identity",
                table: "AccessToken",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Identity",
                table: "AccessToken",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "Identity",
                table: "AccessToken",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");
        }
    }
}
