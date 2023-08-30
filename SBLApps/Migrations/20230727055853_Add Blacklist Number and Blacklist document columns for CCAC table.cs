using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBLApps.Migrations
{
    /// <inheritdoc />
    public partial class AddBlacklistNumberandBlacklistdocumentcolumnsforCCACtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlacklistDocumentFullPath",
                table: "MemoRequestOperations",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BlacklistNumber",
                table: "MemoRequestOperations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlacklistDocumentFullPath",
                table: "MemoRequestOperations");

            migrationBuilder.DropColumn(
                name: "BlacklistNumber",
                table: "MemoRequestOperations");
        }
    }
}
