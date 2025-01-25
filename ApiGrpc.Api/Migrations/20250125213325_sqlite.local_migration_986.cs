using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiGrpc.Api.Migrations
{
    /// <inheritdoc />
    public partial class sqlitelocal_migration_986 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorariosFuncionamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EstabelecimentoId = table.Column<int>(type: "INTEGER", nullable: false),
                    DiaSemana = table.Column<int>(type: "INTEGER", nullable: false),
                    HoraAbertura = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    HoraFechamento = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosFuncionamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estabelecimentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RazaoSocial = table.Column<string>(type: "TEXT", nullable: false),
                    NomeFantasia = table.Column<string>(type: "TEXT", nullable: false),
                    CNPJ = table.Column<string>(type: "TEXT", nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    UrlImagem = table.Column<string>(type: "TEXT", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    InscricaoEstadual = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoMunicipal = table.Column<string>(type: "TEXT", nullable: true),
                    Website = table.Column<string>(type: "TEXT", nullable: true),
                    RedeSocial = table.Column<string>(type: "TEXT", nullable: true),
                    HorarioFuncionamentoId = table.Column<Guid>(type: "TEXT", nullable: true),
                    EnderecoId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CategoriaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estabelecimentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estabelecimentos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Estabelecimentos_Enderecos_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Enderecos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Estabelecimentos_HorariosFuncionamentos_HorarioFuncionamentoId",
                        column: x => x.HorarioFuncionamentoId,
                        principalTable: "HorariosFuncionamentos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estabelecimentos_CategoriaId",
                table: "Estabelecimentos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Estabelecimentos_EnderecoId",
                table: "Estabelecimentos",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Estabelecimentos_HorarioFuncionamentoId",
                table: "Estabelecimentos",
                column: "HorarioFuncionamentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estabelecimentos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "HorariosFuncionamentos");
        }
    }
}
