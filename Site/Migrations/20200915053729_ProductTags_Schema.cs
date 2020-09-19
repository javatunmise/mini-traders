using Microsoft.EntityFrameworkCore.Migrations;

namespace site.Migrations
{
    public partial class ProductTags_Schema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Chat_SiteUsers_InitiatorUserId",
            //    table: "Chat");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Chat_SiteUsers_RecipientUserId",
            //    table: "Chat");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ChatMessage_Chat_ChatId",
            //    table: "ChatMessage");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ChatMessage_SiteUsers_SiteUserId",
            //    table: "ChatMessage");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_ChatMessage",
            //    table: "ChatMessage");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Chat",
            //    table: "Chat");

            //migrationBuilder.RenameTable(
            //    name: "ChatMessage",
            //    newName: "ChatMessages");

            //migrationBuilder.RenameTable(
            //    name: "Chat",
            //    newName: "Chats");

            //migrationBuilder.RenameIndex(
            //    name: "IX_ChatMessage_SiteUserId",
            //    table: "ChatMessages",
            //    newName: "IX_ChatMessages_SiteUserId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_ChatMessage_ChatId",
            //    table: "ChatMessages",
            //    newName: "IX_ChatMessages_ChatId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Chat_RecipientUserId",
            //    table: "Chats",
            //    newName: "IX_Chats_RecipientUserId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Chat_InitiatorUserId",
            //    table: "Chats",
            //    newName: "IX_Chats_InitiatorUserId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_ChatMessages",
            //    table: "ChatMessages",
            //    column: "Id");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Chats",
            //    table: "Chats",
            //    column: "Id");

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTags",
                columns: table => new
                {
                    TagId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTags", x => new { x.ProductId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ProductTags_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTags_TagId",
                table: "ProductTags",
                column: "TagId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ChatMessages_Chats_ChatId",
            //    table: "ChatMessages",
            //    column: "ChatId",
            //    principalTable: "Chats",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ChatMessages_SiteUsers_SiteUserId",
            //    table: "ChatMessages",
            //    column: "SiteUserId",
            //    principalTable: "SiteUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Chats_SiteUsers_InitiatorUserId",
            //    table: "Chats",
            //    column: "InitiatorUserId",
            //    principalTable: "SiteUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Chats_SiteUsers_RecipientUserId",
            //    table: "Chats",
            //    column: "RecipientUserId",
            //    principalTable: "SiteUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ChatMessages_Chats_ChatId",
            //    table: "ChatMessages");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ChatMessages_SiteUsers_SiteUserId",
            //    table: "ChatMessages");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Chats_SiteUsers_InitiatorUserId",
            //    table: "Chats");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Chats_SiteUsers_RecipientUserId",
            //    table: "Chats");

            migrationBuilder.DropTable(
                name: "ProductTags");

            migrationBuilder.DropTable(
                name: "Tags");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Chats",
            //    table: "Chats");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_ChatMessages",
            //    table: "ChatMessages");

            //migrationBuilder.RenameTable(
            //    name: "Chats",
            //    newName: "Chat");

            //migrationBuilder.RenameTable(
            //    name: "ChatMessages",
            //    newName: "ChatMessage");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Chats_RecipientUserId",
            //    table: "Chat",
            //    newName: "IX_Chat_RecipientUserId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Chats_InitiatorUserId",
            //    table: "Chat",
            //    newName: "IX_Chat_InitiatorUserId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_ChatMessages_SiteUserId",
            //    table: "ChatMessage",
            //    newName: "IX_ChatMessage_SiteUserId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_ChatMessages_ChatId",
            //    table: "ChatMessage",
            //    newName: "IX_ChatMessage_ChatId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Chat",
            //    table: "Chat",
            //    column: "Id");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_ChatMessage",
            //    table: "ChatMessage",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Chat_SiteUsers_InitiatorUserId",
            //    table: "Chat",
            //    column: "InitiatorUserId",
            //    principalTable: "SiteUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Chat_SiteUsers_RecipientUserId",
            //    table: "Chat",
            //    column: "RecipientUserId",
            //    principalTable: "SiteUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ChatMessage_Chat_ChatId",
            //    table: "ChatMessage",
            //    column: "ChatId",
            //    principalTable: "Chat",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ChatMessage_SiteUsers_SiteUserId",
            //    table: "ChatMessage",
            //    column: "SiteUserId",
            //    principalTable: "SiteUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
