using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace site.Migrations
{
    public partial class UserAccounts_AccountEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActivatedOn",
                table: "Stores",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenAccountCode",
                table: "SiteUsers",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WalletAccountCode",
                table: "SiteUsers",
                maxLength: 25,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TransactionAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<string>(maxLength: 25, nullable: true),
                    SiteUserId = table.Column<int>(nullable: false),
                    AccountType = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionAccounts_SiteUsers_SiteUserId",
                        column: x => x.SiteUserId,
                        principalTable: "SiteUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionAccountId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Narration = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    EntryType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionEntries_TransactionAccounts_TransactionAccountId",
                        column: x => x.TransactionAccountId,
                        principalTable: "TransactionAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionAccounts_AccountId",
                table: "TransactionAccounts",
                column: "AccountId",
                unique: true,
                filter: "[AccountId] IS NOT NULL")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionAccounts_SiteUserId",
                table: "TransactionAccounts",
                column: "SiteUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionEntries_TransactionAccountId",
                table: "TransactionEntries",
                column: "TransactionAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionEntries");

            migrationBuilder.DropTable(
                name: "TransactionAccounts");

            migrationBuilder.DropColumn(
                name: "ActivatedOn",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "TokenAccountCode",
                table: "SiteUsers");

            migrationBuilder.DropColumn(
                name: "WalletAccountCode",
                table: "SiteUsers");
        }
    }
}
