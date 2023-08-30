using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBLApps.Migrations
{
    public partial class ChangedRecommendertoNextAuthority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Recommender",
                table: "BlacklistingMemoMains",
                newName: "NextAuthority");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NextAuthority",
                table: "BlacklistingMemoMains",
                newName: "Recommender");
        }
    }
}
