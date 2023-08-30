using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBLApps.Migrations
{
    /// <inheritdoc />
    public partial class CorporateDetailtabletoOtherPartydetailtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlacklistingCorporateDetails");

            migrationBuilder.CreateTable(
                name: "BlacklistingOtherPartyDetails",
                columns: table => new
                {
                    OtherPartyDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemoId = table.Column<long>(type: "bigint", nullable: false),
                    SNo = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShareHoldingPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistingOtherPartyDetails", x => x.OtherPartyDetailId);
                    table.ForeignKey(
                        name: "FK_BlacklistingOtherPartyDetails_BlacklistingMemoMains_MemoId",
                        column: x => x.MemoId,
                        principalTable: "BlacklistingMemoMains",
                        principalColumn: "MemoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistingOtherPartyDetails_MemoId",
                table: "BlacklistingOtherPartyDetails",
                column: "MemoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlacklistingOtherPartyDetails");

            migrationBuilder.CreateTable(
                name: "BlacklistingCorporateDetails",
                columns: table => new
                {
                    CorporateDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemoId = table.Column<long>(type: "bigint", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CompanyFullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SNo = table.Column<int>(type: "int", nullable: true),
                    ShareHoldingPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistingCorporateDetails", x => x.CorporateDetailId);
                    table.ForeignKey(
                        name: "FK_BlacklistingCorporateDetails_BlacklistingMemoMains_MemoId",
                        column: x => x.MemoId,
                        principalTable: "BlacklistingMemoMains",
                        principalColumn: "MemoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistingCorporateDetails_MemoId",
                table: "BlacklistingCorporateDetails",
                column: "MemoId");
        }
    }
}
