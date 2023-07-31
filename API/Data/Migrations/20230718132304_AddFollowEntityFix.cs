using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowEntityFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollow_Users_SourceUserId",
                table: "UserFollow");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollow_Users_TargetUserId",
                table: "UserFollow");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFollow",
                table: "UserFollow");

            migrationBuilder.RenameTable(
                name: "UserFollow",
                newName: "Follows");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollow_TargetUserId",
                table: "Follows",
                newName: "IX_Follows_TargetUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Follows",
                table: "Follows",
                columns: new[] { "SourceUserId", "TargetUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Users_SourceUserId",
                table: "Follows",
                column: "SourceUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Users_TargetUserId",
                table: "Follows",
                column: "TargetUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Users_SourceUserId",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Users_TargetUserId",
                table: "Follows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Follows",
                table: "Follows");

            migrationBuilder.RenameTable(
                name: "Follows",
                newName: "UserFollow");

            migrationBuilder.RenameIndex(
                name: "IX_Follows_TargetUserId",
                table: "UserFollow",
                newName: "IX_UserFollow_TargetUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFollow",
                table: "UserFollow",
                columns: new[] { "SourceUserId", "TargetUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollow_Users_SourceUserId",
                table: "UserFollow",
                column: "SourceUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollow_Users_TargetUserId",
                table: "UserFollow",
                column: "TargetUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
