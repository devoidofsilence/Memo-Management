using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBLApps.Migrations
{
    public partial class AddedOperationIdinMainTable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemoRequestOperations_BlacklistingMemoMains_RequestID",
                table: "MemoRequestOperations");

            migrationBuilder.AddForeignKey(
                name: "FK_MemoRequestOperations_BlacklistingMemoMains_RequestID",
                table: "MemoRequestOperations",
                column: "RequestID",
                principalTable: "BlacklistingMemoMains",
                principalColumn: "MemoId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemoRequestOperations_BlacklistingMemoMains_RequestID",
                table: "MemoRequestOperations");

            migrationBuilder.AddForeignKey(
                name: "FK_MemoRequestOperations_BlacklistingMemoMains_RequestID",
                table: "MemoRequestOperations",
                column: "RequestID",
                principalTable: "BlacklistingMemoMains",
                principalColumn: "MemoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
