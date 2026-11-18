using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AlunosService.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alunos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    DataNascimento = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Objetivo = table.Column<string>(type: "TEXT", nullable: false),
                    Nivel = table.Column<string>(type: "TEXT", nullable: false),
                    PesoAtual = table.Column<double>(type: "REAL", nullable: false),
                    AlturaCm = table.Column<int>(type: "INTEGER", nullable: false),
                    MetaPeso = table.Column<double>(type: "REAL", nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alunos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Alunos",
                columns: new[] { "Id", "AlturaCm", "Ativo", "DataNascimento", "Email", "MetaPeso", "Nivel", "Nome", "Objetivo", "PesoAtual" },
                values: new object[,]
                {
                    { 1, 165, true, new DateOnly(1998, 3, 14), "ana.souza@email.com", 65.0, "Iniciante", "Ana Souza", "Emagrecimento", 72.5 },
                    { 2, 180, true, new DateOnly(1995, 7, 2), "bruno.lima@email.com", 84.0, "Intermediario", "Bruno Lima", "Hipertrofia", 78.0 },
                    { 3, 170, true, new DateOnly(1990, 11, 23), "carla.mendes@email.com", null, "Avancado", "Carla Mendes", "Condicionamento", 61.0 },
                    { 4, 175, false, new DateOnly(2001, 1, 9), "diego.rocha@email.com", 76.0, "Iniciante", "Diego Rocha", "Hipertrofia", 70.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_Email",
                table: "Alunos",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alunos");
        }
    }
}
