using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiGrpc.Api.Migrations
{
    /// <inheritdoc />
    public partial class sqlitelocal_migration_651 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estabelecimentos_Categorias_CategoriaId",
                table: "Estabelecimentos");

            migrationBuilder.DropIndex(
                name: "IX_Estabelecimentos_CategoriaId",
                table: "Estabelecimentos");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoriaId1",
                table: "Estabelecimentos",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EstabelecimentoId",
                table: "Categorias",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Estabelecimentos_CategoriaId1",
                table: "Estabelecimentos",
                column: "CategoriaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Estabelecimentos_Categorias_CategoriaId1",
                table: "Estabelecimentos",
                column: "CategoriaId1",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estabelecimentos_Categorias_CategoriaId1",
                table: "Estabelecimentos");

            migrationBuilder.DropIndex(
                name: "IX_Estabelecimentos_CategoriaId1",
                table: "Estabelecimentos");

            migrationBuilder.DropColumn(
                name: "CategoriaId1",
                table: "Estabelecimentos");

            migrationBuilder.DropColumn(
                name: "EstabelecimentoId",
                table: "Categorias");

            migrationBuilder.CreateIndex(
                name: "IX_Estabelecimentos_CategoriaId",
                table: "Estabelecimentos",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estabelecimentos_Categorias_CategoriaId",
                table: "Estabelecimentos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
