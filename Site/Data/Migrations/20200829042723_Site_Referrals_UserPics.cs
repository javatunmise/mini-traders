using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Site.Data.Migrations
{
    public partial class Site_Referrals_UserPics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                table: "SiteUsers",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Referrals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VistorStoreId = table.Column<int>(nullable: false),
                    VisitorStoreId = table.Column<int>(nullable: true),
                    ReferrerUserId = table.Column<int>(nullable: false),
                    ReferralCode = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referrals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Referrals_Stores_VisitorStoreId",
                        column: x => x.VisitorStoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_VisitorStoreId",
                table: "Referrals",
                column: "VisitorStoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Referrals");

            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                table: "SiteUsers");
        }
    }
}
