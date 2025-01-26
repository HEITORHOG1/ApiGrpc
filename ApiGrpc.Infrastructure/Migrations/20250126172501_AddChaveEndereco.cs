using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiGrpc.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChaveEndereco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
               name: "UsuarioId",
               table: "Enderecos",
               nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EstabelecimentoId",
                table: "Enderecos",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}