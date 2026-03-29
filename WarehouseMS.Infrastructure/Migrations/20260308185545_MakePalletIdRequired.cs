using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsyncWarehouse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakePalletIdRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Pallets_PalletId",
                table: "InventoryItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "PalletId",
                table: "InventoryItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Pallets_PalletId",
                table: "InventoryItems",
                column: "PalletId",
                principalTable: "Pallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Pallets_PalletId",
                table: "InventoryItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "PalletId",
                table: "InventoryItems",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Pallets_PalletId",
                table: "InventoryItems",
                column: "PalletId",
                principalTable: "Pallets",
                principalColumn: "Id");
        }
    }
}
