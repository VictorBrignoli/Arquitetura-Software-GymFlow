using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AvaliacoesService.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Avaliacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlunoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Braco = table.Column<double>(type: "REAL", nullable: true),
                    Cintura = table.Column<double>(type: "REAL", nullable: true),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", nullable: true),
                    PercentualGordura = table.Column<double>(type: "REAL", nullable: true),
                    Peso = table.Column<double>(type: "REAL", nullable: false),
                    Quadril = table.Column<double>(type: "REAL", nullable: true),
                    Sincronizada = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacoes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Avaliacoes",
                columns: new[] { "Id", "AlunoId", "Braco", "Cintura", "Data", "Observacoes", "PercentualGordura", "Peso", "Quadril", "Sincronizada" },
                values: new object[,]
                {
                    { 1, 1, 29.0, 84.0, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Avaliação inicial", 31.0, 75.0, 104.0, true },
                    { 2, 1, 29.5, 82.0, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Boa evolução", 29.5, 72.5, 102.0, true },
                    { 3, 2, 35.0, 82.0, new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Avaliação inicial", 16.0, 78.0, 98.0, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avaliacoes");
        }
    }
}
