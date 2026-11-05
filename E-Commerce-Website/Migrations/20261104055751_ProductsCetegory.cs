using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Website.Migrations
{
    /// <inheritdoc />
    public partial class ProductsCetegory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CatId",
                table: "tbl_product");

            migrationBuilder.AddColumn<Guid>(
                name: "Cat_Id",
                table: "tbl_product",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_Cat_Id",
                table: "tbl_product",
                column: "Cat_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_category_Cat_Id",
                table: "tbl_product",
                column: "Cat_Id",
                principalTable: "tbl_category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_category_Cat_Id",
                table: "tbl_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_Cat_Id",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "Cat_Id",
                table: "tbl_product");

            migrationBuilder.AddColumn<int>(
                name: "CatId",
                table: "tbl_product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
