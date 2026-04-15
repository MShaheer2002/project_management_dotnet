using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_management_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateorg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceMembers_OrganizationMemberId",
                table: "WorkspaceMembers",
                column: "OrganizationMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkspaceMembers_OrganizationMembers_OrganizationMemberId",
                table: "WorkspaceMembers",
                column: "OrganizationMemberId",
                principalTable: "OrganizationMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkspaceMembers_OrganizationMembers_OrganizationMemberId",
                table: "WorkspaceMembers");

            migrationBuilder.DropIndex(
                name: "IX_WorkspaceMembers_OrganizationMemberId",
                table: "WorkspaceMembers");
        }
    }
}
