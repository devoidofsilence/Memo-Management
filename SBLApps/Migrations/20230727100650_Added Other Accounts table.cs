using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBLApps.Migrations
{
    /// <inheritdoc />
    public partial class AddedOtherAccountstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OtherAccounts",
                columns: table => new
                {
                    DetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemoId = table.Column<long>(type: "bigint", nullable: false),
                    CIF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountScheme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreezeStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherAccounts", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_OtherAccounts_BlacklistingMemoMains_MemoId",
                        column: x => x.MemoId,
                        principalTable: "BlacklistingMemoMains",
                        principalColumn: "MemoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OtherAccounts_MemoId",
                table: "OtherAccounts",
                column: "MemoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OtherAccounts");
        }
    }
}
