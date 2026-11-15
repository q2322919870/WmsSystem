using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInboundReceiving : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Wms_Inventory",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "Wms_InboundOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Wms_InboundOrder_WarehouseId",
                table: "Wms_InboundOrder",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wms_InboundOrder_Wms_Warehouse_WarehouseId",
                table: "Wms_InboundOrder",
                column: "WarehouseId",
                principalTable: "Wms_Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wms_InboundOrder_Wms_Warehouse_WarehouseId",
                table: "Wms_InboundOrder");

            migrationBuilder.DropIndex(
                name: "IX_Wms_InboundOrder_WarehouseId",
                table: "Wms_InboundOrder");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Wms_Inventory");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Wms_InboundOrder");
        }
    }
}
