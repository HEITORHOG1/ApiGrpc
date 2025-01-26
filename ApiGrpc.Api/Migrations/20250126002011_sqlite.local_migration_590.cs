using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiGrpc.Api.Migrations
{
    /// <inheritdoc />
    public partial class sqlitelocal_migration_590 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estabelecimentos_Enderecos_EnderecoId",
                table: "Estabelecimentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Estabelecimentos_HorariosFuncionamentos_HorarioFuncionamentoId",
                table: "Estabelecimentos");

            migrationBuilder.AlterColumn<Guid>(
                name: "HorarioFuncionamentoId",
                table: "Estabelecimentos",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EnderecoId",
                table: "Estabelecimentos",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Estabelecimentos_Enderecos_EnderecoId",
                table: "Estabelecimentos",
                column: "EnderecoId",
                principalTable: "Enderecos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Estabelecimentos_HorariosFuncionamentos_HorarioFuncionamentoId",
                table: "Estabelecimentos",
                column: "HorarioFuncionamentoId",
                principalTable: "HorariosFuncionamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estabelecimentos_Enderecos_EnderecoId",
                table: "Estabelecimentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Estabelecimentos_HorariosFuncionamentos_HorarioFuncionamentoId",
                table: "Estabelecimentos");

            migrationBuilder.AlterColumn<Guid>(
                name: "HorarioFuncionamentoId",
                table: "Estabelecimentos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "EnderecoId",
                table: "Estabelecimentos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Estabelecimentos_Enderecos_EnderecoId",
                table: "Estabelecimentos",
                column: "EnderecoId",
                principalTable: "Enderecos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Estabelecimentos_HorariosFuncionamentos_HorarioFuncionamentoId",
                table: "Estabelecimentos",
                column: "HorarioFuncionamentoId",
                principalTable: "HorariosFuncionamentos",
                principalColumn: "Id");
        }
    }
}
