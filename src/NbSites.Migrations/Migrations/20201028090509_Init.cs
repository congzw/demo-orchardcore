using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NbSites.Migrations.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "App_Portal_Blog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Portal_Blog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseLib_Products_Product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseLib_Products_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductVersion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    BuildVersion = table.Column<string>(nullable: true),
                    FriendlyVersion = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    CreateAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVersion_Product",
                        column: x => x.ProductId,
                        principalTable: "BaseLib_Products_Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVersion_ProductId",
                table: "ProductVersion",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "App_Portal_Blog");

            migrationBuilder.DropTable(
                name: "ProductVersion");

            migrationBuilder.DropTable(
                name: "BaseLib_Products_Product");
        }
    }
}
