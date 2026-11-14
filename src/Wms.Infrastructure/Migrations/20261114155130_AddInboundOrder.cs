using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInboundOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wms_InboundOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExpectedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wms_InboundOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wms_InboundOrderDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    ExpectedQty = table.Column<int>(type: "int", nullable: false),
                    ReceivedQty = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wms_InboundOrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wms_InboundOrderDetail_Wms_InboundOrder_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Wms_InboundOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wms_InboundOrderDetail_Wms_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Wms_Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Wms_InboundOrderDetail_Wms_StorageLocation_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Wms_StorageLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wms_InboundOrder_OrderNo",
                table: "Wms_InboundOrder",
                column: "OrderNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wms_InboundOrderDetail_LocationId",
                table: "Wms_InboundOrderDetail",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Wms_InboundOrderDetail_MaterialId",
                table: "Wms_InboundOrderDetail",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Wms_InboundOrderDetail_OrderId",
                table: "Wms_InboundOrderDetail",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wms_InboundOrderDetail");

            migrationBuilder.DropTable(
                name: "Wms_InboundOrder");
        }
    }
}
