using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class PostUserJoinTableRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostUser_AspNetUsers_LikedById",
                table: "PostUser");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUser_Posts_LikedPostsId",
                table: "PostUser");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUser1_AspNetUsers_DislikedById",
                table: "PostUser1");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUser1_Posts_DislikedPostsId",
                table: "PostUser1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostUser1",
                table: "PostUser1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostUser",
                table: "PostUser");

            migrationBuilder.RenameTable(
                name: "PostUser1",
                newName: "PostUserDislike");

            migrationBuilder.RenameTable(
                name: "PostUser",
                newName: "PostUserLike");

            migrationBuilder.RenameIndex(
                name: "IX_PostUser1_DislikedPostsId",
                table: "PostUserDislike",
                newName: "IX_PostUserDislike_DislikedPostsId");

            migrationBuilder.RenameIndex(
                name: "IX_PostUser_LikedPostsId",
                table: "PostUserLike",
                newName: "IX_PostUserLike_LikedPostsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostUserDislike",
                table: "PostUserDislike",
                columns: new[] { "DislikedById", "DislikedPostsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostUserLike",
                table: "PostUserLike",
                columns: new[] { "LikedById", "LikedPostsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PostUserDislike_AspNetUsers_DislikedById",
                table: "PostUserDislike",
                column: "DislikedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUserDislike_Posts_DislikedPostsId",
                table: "PostUserDislike",
                column: "DislikedPostsId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUserLike_AspNetUsers_LikedById",
                table: "PostUserLike",
                column: "LikedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUserLike_Posts_LikedPostsId",
                table: "PostUserLike",
                column: "LikedPostsId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostUserDislike_AspNetUsers_DislikedById",
                table: "PostUserDislike");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUserDislike_Posts_DislikedPostsId",
                table: "PostUserDislike");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUserLike_AspNetUsers_LikedById",
                table: "PostUserLike");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUserLike_Posts_LikedPostsId",
                table: "PostUserLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostUserLike",
                table: "PostUserLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostUserDislike",
                table: "PostUserDislike");

            migrationBuilder.RenameTable(
                name: "PostUserLike",
                newName: "PostUser");

            migrationBuilder.RenameTable(
                name: "PostUserDislike",
                newName: "PostUser1");

            migrationBuilder.RenameIndex(
                name: "IX_PostUserLike_LikedPostsId",
                table: "PostUser",
                newName: "IX_PostUser_LikedPostsId");

            migrationBuilder.RenameIndex(
                name: "IX_PostUserDislike_DislikedPostsId",
                table: "PostUser1",
                newName: "IX_PostUser1_DislikedPostsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostUser",
                table: "PostUser",
                columns: new[] { "LikedById", "LikedPostsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostUser1",
                table: "PostUser1",
                columns: new[] { "DislikedById", "DislikedPostsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PostUser_AspNetUsers_LikedById",
                table: "PostUser",
                column: "LikedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUser_Posts_LikedPostsId",
                table: "PostUser",
                column: "LikedPostsId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUser1_AspNetUsers_DislikedById",
                table: "PostUser1",
                column: "DislikedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUser1_Posts_DislikedPostsId",
                table: "PostUser1",
                column: "DislikedPostsId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
