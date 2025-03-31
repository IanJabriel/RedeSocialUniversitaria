using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RedeSocialUniversidade.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Usuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Postagens_PostagemId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Comentario");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Postagens");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_PostagemId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PostagemId",
                table: "Usuarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostagemId",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExigeInscricao = table.Column<bool>(type: "bit", nullable: false),
                    Local = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Postagens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    Conteudo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postagens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Postagens_Usuarios_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    PostagemId = table.Column<int>(type: "int", nullable: true),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comentario_Postagens_PostagemId",
                        column: x => x.PostagemId,
                        principalTable: "Postagens",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comentario_Usuarios_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PostagemId",
                table: "Usuarios",
                column: "PostagemId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_AutorId",
                table: "Comentario",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_PostagemId",
                table: "Comentario",
                column: "PostagemId");

            migrationBuilder.CreateIndex(
                name: "IX_Postagens_AutorId",
                table: "Postagens",
                column: "AutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Postagens_PostagemId",
                table: "Usuarios",
                column: "PostagemId",
                principalTable: "Postagens",
                principalColumn: "Id");
        }
    }
}
