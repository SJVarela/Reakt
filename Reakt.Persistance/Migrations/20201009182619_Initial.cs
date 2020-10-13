using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reakt.Persistance.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(maxLength: 600, nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    BoardId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(maxLength: 600, nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    Likes = table.Column<int>(nullable: false),
                    Message = table.Column<string>(maxLength: 4000, nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    PostId = table.Column<long>(nullable: false),
                    ReplyCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Active", "CreatedAt", "DeletedAt", "Description", "Title", "UpdatedAt" },
                values: new object[] { 1L, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "This is a seeded board", "Seed bored", null });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Active", "BoardId", "CreatedAt", "DeletedAt", "Description", "Title", "UpdatedAt" },
                values: new object[] { 1L, false, 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "This is a seeded post", "Seed post", null });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Active", "CreatedAt", "DeletedAt", "Likes", "Message", "ParentId", "PostId", "ReplyCount", "UpdatedAt" },
                values: new object[] { 1L, true, new DateTime(2020, 10, 9, 15, 26, 19, 486, DateTimeKind.Local).AddTicks(1974), null, 0, "This post sucks", null, 1L, 0, null });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Active", "CreatedAt", "DeletedAt", "Likes", "Message", "ParentId", "PostId", "ReplyCount", "UpdatedAt" },
                values: new object[] { 2L, true, new DateTime(2020, 10, 9, 15, 26, 19, 487, DateTimeKind.Local).AddTicks(1217), null, 0, "This post is good", null, 1L, 0, null });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Active", "CreatedAt", "DeletedAt", "Likes", "Message", "ParentId", "PostId", "ReplyCount", "UpdatedAt" },
                values: new object[] { 3L, true, new DateTime(2020, 10, 9, 15, 26, 19, 487, DateTimeKind.Local).AddTicks(1244), null, 0, "This comment is good", 1L, 1L, 0, null });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentId",
                table: "Comments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BoardId",
                table: "Posts",
                column: "BoardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Boards");
        }
    }
}
