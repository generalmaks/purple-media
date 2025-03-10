using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace purple_media_rest.Migrations
{
    /// <inheritdoc />
    public partial class AddedComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            // migrationBuilder.CreateTable(
            //     name: "Comment",
            //     columns: table => new
            //     {
            //         CommentId = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //         PostId = table.Column<int>(type: "int", nullable: false),
            //         Content = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
            //             .Annotation("MySql:CharSet", "utf8mb4"),
            //         AuthorId = table.Column<int>(type: "int", nullable: false),
            //         CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Comment", x => x.CommentId);
            //     })
            //     .Annotation("MySql:CharSet", "utf8mb4");

            // migrationBuilder.CreateTable(
            //     name: "Posts",
            //     columns: table => new
            //     {
            //         PostId = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //         Content = table.Column<string>(type: "longtext", nullable: false)
            //             .Annotation("MySql:CharSet", "utf8mb4"),
            //         CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //         UserId = table.Column<int>(type: "int", nullable: false),
            //         CommentsCount = table.Column<int>(type: "int", nullable: false),
            //         Likes = table.Column<int>(type: "int", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Posts", x => x.PostId);
            //     })
            //     .Annotation("MySql:CharSet", "utf8mb4");

            // migrationBuilder.CreateTable(
            //     name: "Users",
            //     columns: table => new
            //     {
            //         UserId = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //         Email = table.Column<string>(type: "varchar(255)", nullable: false)
            //             .Annotation("MySql:CharSet", "utf8mb4"),
            //         Username = table.Column<string>(type: "varchar(255)", nullable: false)
            //             .Annotation("MySql:CharSet", "utf8mb4"),
            //         PasswordHash = table.Column<string>(type: "longtext", nullable: false)
            //             .Annotation("MySql:CharSet", "utf8mb4"),
            //         CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //         PostId = table.Column<int>(type: "int", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Users", x => x.UserId);
            //         table.ForeignKey(
            //             name: "FK_Users_Posts_PostId",
            //             column: x => x.PostId,
            //             principalTable: "Posts",
            //             principalColumn: "PostId");
            //     })
            //     .Annotation("MySql:CharSet", "utf8mb4");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Comment_AuthorId",
            //     table: "Comment",
            //     column: "AuthorId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Comment_PostId",
            //     table: "Comment",
            //     column: "PostId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Posts_UserId",
            //     table: "Posts",
            //     column: "UserId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Users_Email",
            //     table: "Users",
            //     column: "Email",
            //     unique: true);

            // migrationBuilder.CreateIndex(
            //     name: "IX_Users_PostId",
            //     table: "Users",
            //     column: "PostId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Users_Username",
            //     table: "Users",
            //     column: "Username",
            //     unique: true);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Comment_Posts_PostId",
            //     table: "Comment",
            //     column: "PostId",
            //     principalTable: "Posts",
            //     principalColumn: "PostId",
            //     onDelete: ReferentialAction`.Cascade);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Comment_Users_AuthorId",
            //     table: "Comment",
            //     column: "AuthorId",
            //     principalTable: "Users",
            //     principalColumn: "UserId",
            //     onDelete: ReferentialAction.Cascade);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Posts_Users_UserId",
            //     table: "Posts",
            //     column: "UserId",
            //     principalTable: "Users",
            //     principalColumn: "UserId",
            //     onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Posts_PostId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
