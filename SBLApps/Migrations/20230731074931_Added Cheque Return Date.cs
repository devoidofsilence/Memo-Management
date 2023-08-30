﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBLApps.Migrations
{
    /// <inheritdoc />
    public partial class AddedChequeReturnDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChequeReturnDate",
                table: "BlacklistingMemoDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChequeReturnDate",
                table: "BlacklistingMemoDetails");
        }
    }
}
