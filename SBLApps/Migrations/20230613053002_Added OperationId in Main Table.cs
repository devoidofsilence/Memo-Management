using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBLApps.Migrations
{
    public partial class AddedOperationIdinMainTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlacklistingMemoMains_Operations_CurrentOperationId",
                table: "BlacklistingMemoMains");

            migrationBuilder.DropForeignKey(
                name: "FK_BlacklistingMemoMains_Operations_NextOperationId",
                table: "BlacklistingMemoMains");

            migrationBuilder.DropIndex(
                name: "IX_BlacklistingMemoMains_CurrentOperationId",
                table: "BlacklistingMemoMains");

            migrationBuilder.DropColumn(
                name: "CurrentOperationId",
                table: "BlacklistingMemoMains");

            migrationBuilder.RenameColumn(
                name: "NextOperationId",
                table: "BlacklistingMemoMains",
                newName: "LatestOperationId");

            migrationBuilder.RenameIndex(
                name: "IX_BlacklistingMemoMains_NextOperationId",
                table: "BlacklistingMemoMains",
                newName: "IX_BlacklistingMemoMains_LatestOperationId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlacklistingMemoMains_Operations_LatestOperationId",
                table: "BlacklistingMemoMains",
                column: "LatestOperationId",
                principalTable: "Operations",
                principalColumn: "OperationID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlacklistingMemoMains_Operations_LatestOperationId",
                table: "BlacklistingMemoMains");

            migrationBuilder.RenameColumn(
                name: "LatestOperationId",
                table: "BlacklistingMemoMains",
                newName: "NextOperationId");

            migrationBuilder.RenameIndex(
                name: "IX_BlacklistingMemoMains_LatestOperationId",
                table: "BlacklistingMemoMains",
                newName: "IX_BlacklistingMemoMains_NextOperationId");

            migrationBuilder.AddColumn<int>(
                name: "CurrentOperationId",
                table: "BlacklistingMemoMains",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistingMemoMains_CurrentOperationId",
                table: "BlacklistingMemoMains",
                column: "CurrentOperationId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlacklistingMemoMains_Operations_CurrentOperationId",
                table: "BlacklistingMemoMains",
                column: "CurrentOperationId",
                principalTable: "Operations",
                principalColumn: "OperationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlacklistingMemoMains_Operations_NextOperationId",
                table: "BlacklistingMemoMains",
                column: "NextOperationId",
                principalTable: "Operations",
                principalColumn: "OperationID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
