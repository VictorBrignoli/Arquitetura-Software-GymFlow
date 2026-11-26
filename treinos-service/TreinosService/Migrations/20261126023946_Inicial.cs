using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TreinosService.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Treinos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlunoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treinos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreinoItens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TreinoId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExercicioId = table.Column<int>(type: "INTEGER", nullable: false),
                    Series = table.Column<int>(type: "INTEGER", nullable: false),
                    Repeticoes = table.Column<int>(type: "INTEGER", nullable: false),
                    DescansoSegundos = table.Column<int>(type: "INTEGER", nullable: false),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreinoItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreinoItens_Treinos_TreinoId",
                        column: x => x.TreinoId,
                        principalTable: "Treinos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Treinos",
                columns: new[] { "Id", "AlunoId", "Ativo", "DataCriacao", "Nome" },
                values: new object[,]
                {
                    { 1, 2, true, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Treino A - Peito e Tríceps" },
                    { 2, 1, true, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Treino Full Body" }
                });

            migrationBuilder.InsertData(
                table: "TreinoItens",
                columns: new[] { "Id", "DescansoSegundos", "ExercicioId", "Ordem", "Repeticoes", "Series", "TreinoId" },
                values: new object[,]
                {
                    { 1, 90, 1, 1, 10, 4, 1 },
                    { 2, 75, 2, 2, 12, 3, 1 },
                    { 3, 60, 29, 3, 12, 3, 1 },
                    { 4, 60, 12, 1, 15, 3, 2 },
                    { 5, 60, 6, 2, 12, 3, 2 },
                    { 6, 45, 33, 3, 20, 3, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreinoItens_TreinoId",
                table: "TreinoItens",
                column: "TreinoId");

            migrationBuilder.CreateIndex(
                name: "IX_Treinos_AlunoId",
                table: "Treinos",
                column: "AlunoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreinoItens");

            migrationBuilder.DropTable(
                name: "Treinos");
        }
    }
}
