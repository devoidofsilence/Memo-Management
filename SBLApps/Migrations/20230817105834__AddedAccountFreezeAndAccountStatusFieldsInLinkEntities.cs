using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBLApps.Migrations
{
    /// <inheritdoc />
    public partial class _AddedAccountFreezeAndAccountStatusFieldsInLinkEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountStatus",
                table: "LinkedEntitiesDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FreezeStatus",
                table: "LinkedEntitiesDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountStatus",
                table: "LinkedEntitiesDetails");

            migrationBuilder.DropColumn(
                name: "FreezeStatus",
                table: "LinkedEntitiesDetails");
        }
    }
}
