using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wms_Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    LockedQty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wms_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wms_Inventory_Wms_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Wms_Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Wms_Inventory_Wms_StorageLocation_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Wms_StorageLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Wms_Inventory_Wms_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Wms_Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Wms_InventoryTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    QtyChange = table.Column<int>(type: "int", nullable: false),
                    BeforeQty = table.Column<int>(type: "int", nullable: false),
                    AfterQty = table.Column<int>(type: "int", nullable: false),
                    OperatorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wms_InventoryTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wms_InventoryTransaction_Wms_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Wms_Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Wms_InventoryTransaction_Wms_StorageLocation_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Wms_StorageLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Wms_InventoryTransaction_Wms_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Wms_Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wms_Inventory_LocationId_MaterialId",
                table: "Wms_Inventory",
                columns: new[] { "LocationId", "MaterialId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wms_Inventory_MaterialId",
                table: "Wms_Inventory",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Wms_Inventory_WarehouseId",
                table: "Wms_Inventory",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Wms_InventoryTransaction_LocationId",
                table: "Wms_InventoryTransaction",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Wms_InventoryTransaction_MaterialId",
                table: "Wms_InventoryTransaction",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Wms_InventoryTransaction_ReferenceNo",
                table: "Wms_InventoryTransaction",
                column: "ReferenceNo");

            migrationBuilder.CreateIndex(
                name: "IX_Wms_InventoryTransaction_WarehouseId",
                table: "Wms_InventoryTransaction",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wms_Inventory");

            migrationBuilder.DropTable(
                name: "Wms_InventoryTransaction");
        }
    }
}
