using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBLApps.Migrations
{
    public partial class NewDatabaseinitialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTypes",
                columns: table => new
                {
                    CustomerTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTypes", x => x.CustomerTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.DocumentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "MemoTypes",
                columns: table => new
                {
                    MemoTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemoTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoTypes", x => x.MemoTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    OperationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.OperationID);
                });

            migrationBuilder.CreateTable(
                name: "RequestStatuses",
                columns: table => new
                {
                    RequestStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestStatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStatuses", x => x.RequestStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

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
                name: "BlacklistingMemoMains",
                columns: table => new
                {
                    MemoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FinalApproverSAMName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MemoTypeId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AccountHolderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerTypeId = table.Column<int>(type: "int", nullable: false),
                    IsLoanCustomer = table.Column<bool>(type: "bit", nullable: false),
                    TotalLoanOutstanding = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NameOfRORM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameOfPayee = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BlacklistingRequestedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BlacklistingApplicationReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddressOfRequestingPerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StaffIDNoOfRequestingPerson = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ContactNumberOfRequestingPerson = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalChequeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequestStatusId = table.Column<int>(type: "int", nullable: false),
                    CurrentOperationId = table.Column<int>(type: "int", nullable: false),
                    NextOperationId = table.Column<int>(type: "int", nullable: false),
                    Recommender = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistingMemoMains", x => x.MemoId);
                    table.ForeignKey(
                        name: "FK_BlacklistingMemoMains_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlacklistingMemoMains_CustomerTypes_CustomerTypeId",
                        column: x => x.CustomerTypeId,
                        principalTable: "CustomerTypes",
                        principalColumn: "CustomerTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlacklistingMemoMains_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlacklistingMemoMains_MemoTypes_MemoTypeId",
                        column: x => x.MemoTypeId,
                        principalTable: "MemoTypes",
                        principalColumn: "MemoTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlacklistingMemoMains_Operations_CurrentOperationId",
                        column: x => x.CurrentOperationId,
                        principalTable: "Operations",
                        principalColumn: "OperationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlacklistingMemoMains_Operations_NextOperationId",
                        column: x => x.NextOperationId,
                        principalTable: "Operations",
                        principalColumn: "OperationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlacklistingMemoMains_RequestStatuses_RequestStatusId",
                        column: x => x.RequestStatusId,
                        principalTable: "RequestStatuses",
                        principalColumn: "RequestStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlacklistingCorporateDetails",
                columns: table => new
                {
                    CorporateDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemoId = table.Column<long>(type: "bigint", nullable: false),
                    SNo = table.Column<int>(type: "int", nullable: true),
                    CompanyFullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShareHoldingPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "BlacklistingDocumentDetails",
                columns: table => new
                {
                    DocumentDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemoId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: true),
                    DocumentFullPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistingDocumentDetails", x => x.DocumentDetailId);
                    table.ForeignKey(
                        name: "FK_BlacklistingDocumentDetails_BlacklistingMemoMains_MemoId",
                        column: x => x.MemoId,
                        principalTable: "BlacklistingMemoMains",
                        principalColumn: "MemoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlacklistingMemoDetails",
                columns: table => new
                {
                    MemoDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemoId = table.Column<long>(type: "bigint", nullable: false),
                    SNo = table.Column<int>(type: "int", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ChequeNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChequeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReasonOfReturn = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistingMemoDetails", x => x.MemoDetailId);
                    table.ForeignKey(
                        name: "FK_BlacklistingMemoDetails_BlacklistingMemoMains_MemoId",
                        column: x => x.MemoId,
                        principalTable: "BlacklistingMemoMains",
                        principalColumn: "MemoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemoRequestOperations",
                columns: table => new
                {
                    MemoRequestOperationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestID = table.Column<long>(type: "bigint", nullable: false),
                    OperationID = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoRequestOperations", x => x.MemoRequestOperationId);
                    table.ForeignKey(
                        name: "FK_MemoRequestOperations_BlacklistingMemoMains_RequestID",
                        column: x => x.RequestID,
                        principalTable: "BlacklistingMemoMains",
                        principalColumn: "MemoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemoRequestOperations_Operations_OperationID",
                        column: x => x.OperationID,
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
                    CIF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AccountScheme = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Balance = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccountStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "IX_BlacklistingCorporateDetails_MemoId",
                table: "BlacklistingCorporateDetails",
                column: "MemoId");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistingDocumentDetails_MemoId",
                table: "BlacklistingDocumentDetails",
                column: "MemoId");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistingMemoDetails_MemoId",
                table: "BlacklistingMemoDetails",
                column: "MemoId");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistingMemoMains_BranchId",
                table: "BlacklistingMemoMains",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistingMemoMains_CurrentOperationId",
                table: "BlacklistingMemoMains",
                column: "CurrentOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistingMemoMains_CustomerTypeId",
                table: "BlacklistingMemoMains",
                column: "CustomerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistingMemoMains_DepartmentId",
                table: "BlacklistingMemoMains",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistingMemoMains_MemoTypeId",
                table: "BlacklistingMemoMains",
                column: "MemoTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistingMemoMains_NextOperationId",
                table: "BlacklistingMemoMains",
                column: "NextOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistingMemoMains_ReferenceNumber",
                table: "BlacklistingMemoMains",
                column: "ReferenceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistingMemoMains_RequestStatusId",
                table: "BlacklistingMemoMains",
                column: "RequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MemoRequestOperations_OperationID",
                table: "MemoRequestOperations",
                column: "OperationID");

            migrationBuilder.CreateIndex(
                name: "IX_MemoRequestOperations_RequestID",
                table: "MemoRequestOperations",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_NextOperationMappers_NextOperationID",
                table: "NextOperationMappers",
                column: "NextOperationID");

            migrationBuilder.CreateIndex(
                name: "IX_OtherAccounts_MemoId",
                table: "OtherAccounts",
                column: "MemoId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlacklistingCorporateDetails");

            migrationBuilder.DropTable(
                name: "BlacklistingDocumentDetails");

            migrationBuilder.DropTable(
                name: "BlacklistingMemoDetails");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "MemoRequestOperations");

            migrationBuilder.DropTable(
                name: "NextOperationMappers");

            migrationBuilder.DropTable(
                name: "OtherAccounts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "BlacklistingMemoMains");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "CustomerTypes");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "MemoTypes");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "RequestStatuses");
        }
    }
}
