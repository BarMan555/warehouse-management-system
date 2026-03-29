using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsyncWarehouse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaxCapacity = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pallets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PalletId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ItemType = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    Hazard = table.Column<int>(type: "integer", nullable: true),
                    ExpirationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    VolumeLiters = table.Column<float>(type: "real", nullable: true),
                    SerialNumber = table.Column<string>(type: "text", nullable: true),
                    WarrantyMonths = table.Column<int>(type: "integer", nullable: true),
                    PowerWatts = table.Column<float>(type: "real", nullable: true),
                    Material = table.Column<string>(type: "text", nullable: true),
                    Dimensions_Length = table.Column<float>(type: "real", nullable: true),
                    Dimensions_Width = table.Column<float>(type: "real", nullable: true),
                    Dimensions_Height = table.Column<float>(type: "real", nullable: true),
                    RequiresAssembly = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Pallets_PalletId",
                        column: x => x.PalletId,
                        principalTable: "Pallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_PalletId",
                table: "InventoryItems",
                column: "PalletId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "Pallets");
        }
    }
}
