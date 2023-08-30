using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBLApps.Migrations
{
    public partial class CleanTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NextOperationMappers");

            migrationBuilder.DropTable(
                name: "OtherAccounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NextOperationMappers",
                columns: table => new
                {
                    CurrentOperationID = table.Column<int>(type: "int", nullable: false),
                    NextOperationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NextOperationMappers", x => new { x.CurrentOperationID, x.NextOperationID });
                    table.ForeignKey(
                        name: "FK_NextOperationMappers_Operations_CurrentOperationID",
                        column: x => x.CurrentOperationID,
                        principalTable: "Operations",
                        principalColumn: "OperationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NextOperationMappers_Operations_NextOperationID",
                        column: x => x.NextOperationID,
                        principalTable: "Operations",
                        principalColumn: "OperationID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OtherAccounts",
                columns: table => new
                {
                    DetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemoId = table.Column<long>(type: "bigint", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AccountScheme = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AccountStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CIF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FreezeStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
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
                name: "IX_NextOperationMappers_NextOperationID",
                table: "NextOperationMappers",
                column: "NextOperationID");

            migrationBuilder.CreateIndex(
                name: "IX_OtherAccounts_MemoId",
                table: "OtherAccounts",
                column: "MemoId");
        }
    }
}
