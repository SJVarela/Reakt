using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Reakt.Persistance.Migrations
{
    public partial class seed : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Boards_BoardId",
                table: "Posts");

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Boards",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.AlterColumn<long>(
                name: "BoardId",
                table: "Posts",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "PostId",
                table: "Comments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Boards_BoardId",
                table: "Posts",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Boards_BoardId",
                table: "Posts");

            migrationBuilder.AlterColumn<long>(
                name: "BoardId",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PostId",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

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
                columns: new[] { "Id", "Active", "CreatedAt", "DeletedAt", "Likes", "Message", "ParentId", "PostId", "UpdatedAt" },
                values: new object[] { 1L, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "This post sucks", null, 1L, null });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Active", "CreatedAt", "DeletedAt", "Likes", "Message", "ParentId", "PostId", "UpdatedAt" },
                values: new object[] { 2L, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "You suck", 1L, 1L, null });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Boards_BoardId",
                table: "Posts",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}