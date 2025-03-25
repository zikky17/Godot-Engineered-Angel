using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngineeredAngel.Migrations
{
    /// <inheritdoc />
    public partial class Updateddatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LootItems_Inventory_InventoryId",
                table: "LootItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LootItems_Inventory_InventoryId1",
                table: "LootItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LootItems",
                table: "LootItems");

            migrationBuilder.RenameTable(
                name: "LootItems",
                newName: "LootItemEntity");

            migrationBuilder.RenameIndex(
                name: "IX_LootItems_InventoryId1",
                table: "LootItemEntity",
                newName: "IX_LootItemEntity_InventoryId1");

            migrationBuilder.RenameIndex(
                name: "IX_LootItems_InventoryId",
                table: "LootItemEntity",
                newName: "IX_LootItemEntity_InventoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LootItemEntity",
                table: "LootItemEntity",
                column: "LootItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_LootItemEntity_Inventory_InventoryId",
                table: "LootItemEntity",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "InventoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LootItemEntity_Inventory_InventoryId1",
                table: "LootItemEntity",
                column: "InventoryId1",
                principalTable: "Inventory",
                principalColumn: "InventoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LootItemEntity_Inventory_InventoryId",
                table: "LootItemEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_LootItemEntity_Inventory_InventoryId1",
                table: "LootItemEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LootItemEntity",
                table: "LootItemEntity");

            migrationBuilder.RenameTable(
                name: "LootItemEntity",
                newName: "LootItems");

            migrationBuilder.RenameIndex(
                name: "IX_LootItemEntity_InventoryId1",
                table: "LootItems",
                newName: "IX_LootItems_InventoryId1");

            migrationBuilder.RenameIndex(
                name: "IX_LootItemEntity_InventoryId",
                table: "LootItems",
                newName: "IX_LootItems_InventoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LootItems",
                table: "LootItems",
                column: "LootItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_LootItems_Inventory_InventoryId",
                table: "LootItems",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "InventoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LootItems_Inventory_InventoryId1",
                table: "LootItems",
                column: "InventoryId1",
                principalTable: "Inventory",
                principalColumn: "InventoryId");
        }
    }
}
