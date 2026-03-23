using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_management_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeInviteTokenNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InviteToken",
                table: "OrganizationMembers",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OrganizationMembers",
                keyColumn: "InviteToken",
                keyValue: null,
                column: "InviteToken",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "InviteToken",
                table: "OrganizationMembers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
