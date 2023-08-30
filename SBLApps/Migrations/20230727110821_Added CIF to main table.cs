using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBLApps.Migrations
{
    /// <inheritdoc />
    public partial class AddedCIFtomaintable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CIF",
                table: "BlacklistingMemoMains",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "LinkedEntitiesDetails",
                columns: table => new
                {
                    DetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemoId = table.Column<long>(type: "bigint", nullable: false),
                    MainAccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cif = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Balance = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkedEntitiesDetails", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_LinkedEntitiesDetails_BlacklistingMemoMains_MemoId",
                        column: x => x.MemoId,
                        principalTable: "BlacklistingMemoMains",
                        principalColumn: "MemoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LinkedEntitiesDetails_MemoId",
                table: "LinkedEntitiesDetails",
                column: "MemoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkedEntitiesDetails");

            migrationBuilder.DropColumn(
                name: "CIF",
                table: "BlacklistingMemoMains");
        }
    }
}
