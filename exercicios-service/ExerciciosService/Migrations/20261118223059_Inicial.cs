using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExerciciosService.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercicios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    GrupoMuscular = table.Column<string>(type: "TEXT", nullable: false),
                    Equipamento = table.Column<string>(type: "TEXT", nullable: false),
                    NivelDificuldade = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercicios", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Exercicios",
                columns: new[] { "Id", "Ativo", "Descricao", "Equipamento", "GrupoMuscular", "NivelDificuldade", "Nome" },
                values: new object[,]
                {
                    { 1, true, "Deitado no banco reto, descer a barra até o peito e empurrar até estender os braços.", "Barra", "Peito", "Intermediario", "Supino reto com barra" },
                    { 2, true, "No banco inclinado a 30-45 graus, empurrar os halteres para cima, focando na parte superior do peito.", "Halteres", "Peito", "Intermediario", "Supino inclinado com halteres" },
                    { 3, true, "Deitado, abrir os braços com cotovelos levemente flexionados e fechar sobre o peito.", "Halteres", "Peito", "Iniciante", "Crucifixo com halteres" },
                    { 4, true, "De pé entre as polias, puxar os cabos à frente do corpo cruzando as mãos.", "Polia", "Peito", "Intermediario", "Crossover na polia" },
                    { 5, true, "Com mãos no chão na largura dos ombros, descer o corpo alinhado e empurrar de volta.", "Peso corporal", "Peito", "Iniciante", "Flexão de braço" },
                    { 6, true, "Sentado no pulley, puxar a barra até a altura do peito, aproximando as escápulas.", "Polia", "Costas", "Iniciante", "Puxada frontal" },
                    { 7, true, "Tronco inclinado à frente, puxar a barra em direção ao abdômen mantendo a coluna neutra.", "Barra", "Costas", "Intermediario", "Remada curvada com barra" },
                    { 8, true, "Sentado, puxar o triângulo em direção ao abdômen mantendo o tronco ereto.", "Polia", "Costas", "Iniciante", "Remada baixa na polia" },
                    { 9, true, "Pendurado na barra, puxar o corpo até o queixo passar a barra.", "Peso corporal", "Costas", "Avancado", "Barra fixa" },
                    { 10, true, "Levantar a barra do chão estendendo quadril e joelhos, com a coluna neutra.", "Barra", "Costas", "Avancado", "Levantamento terra" },
                    { 11, true, "Barra apoiada no trapézio, agachar até as coxas ficarem paralelas ao chão e subir.", "Barra", "Pernas", "Intermediario", "Agachamento livre" },
                    { 12, true, "Empurrar a plataforma com os pés afastados na largura do quadril, sem travar os joelhos.", "Máquina", "Pernas", "Iniciante", "Leg press 45" },
                    { 13, true, "Sentado, estender os joelhos até as pernas ficarem retas, isolando o quadríceps.", "Máquina", "Pernas", "Iniciante", "Cadeira extensora" },
                    { 14, true, "Deitado de bruços, flexionar os joelhos levando os calcanhares em direção aos glúteos.", "Máquina", "Pernas", "Iniciante", "Mesa flexora" },
                    { 15, true, "Dar um passo à frente e descer até o joelho de trás quase tocar o chão, alternando as pernas.", "Halteres", "Pernas", "Intermediario", "Afundo com halteres" },
                    { 16, true, "Elevar os calcanhares o máximo possível e descer devagar, alongando a panturrilha.", "Máquina", "Pernas", "Iniciante", "Panturrilha em pé" },
                    { 17, true, "Costas apoiadas no banco, elevar o quadril com a barra sobre a pelve, contraindo os glúteos.", "Barra", "Gluteos", "Intermediario", "Elevação pélvica com barra" },
                    { 18, true, "Com a caneleira presa à polia, estender o quadril levando a perna para trás.", "Polia", "Gluteos", "Iniciante", "Glúteo na polia" },
                    { 19, true, "Sentado, afastar as pernas contra a resistência da máquina.", "Máquina", "Gluteos", "Iniciante", "Cadeira abdutora" },
                    { 20, true, "Sentado, empurrar os halteres acima da cabeça a partir da altura dos ombros.", "Halteres", "Ombros", "Iniciante", "Desenvolvimento com halteres" },
                    { 21, true, "De pé, elevar os halteres lateralmente até a altura dos ombros.", "Halteres", "Ombros", "Iniciante", "Elevação lateral" },
                    { 22, true, "De pé, elevar os halteres à frente do corpo até a altura dos ombros.", "Halteres", "Ombros", "Iniciante", "Elevação frontal" },
                    { 23, true, "De pé, empurrar a barra da altura do peito até acima da cabeça, com o core firme.", "Barra", "Ombros", "Avancado", "Desenvolvimento militar com barra" },
                    { 24, true, "No peck deck de frente para o encosto, abrir os braços para trabalhar o deltoide posterior.", "Máquina", "Ombros", "Iniciante", "Crucifixo inverso" },
                    { 25, true, "De pé, flexionar os cotovelos levando a barra até os ombros, sem balançar o tronco.", "Barra", "Biceps", "Iniciante", "Rosca direta com barra" },
                    { 26, true, "Flexionar um braço de cada vez, girando o punho para cima durante a subida.", "Halteres", "Biceps", "Iniciante", "Rosca alternada" },
                    { 27, true, "Flexionar os cotovelos com a pegada neutra (palmas voltadas uma para a outra).", "Halteres", "Biceps", "Iniciante", "Rosca martelo" },
                    { 28, true, "Com os braços apoiados no banco Scott, flexionar os cotovelos isolando o bíceps.", "Máquina", "Biceps", "Intermediario", "Rosca Scott" },
                    { 29, true, "De pé na polia alta, estender os cotovelos empurrando a barra para baixo.", "Polia", "Triceps", "Iniciante", "Tríceps pulley" },
                    { 30, true, "Deitado, descer a barra em direção à testa flexionando apenas os cotovelos e estender.", "Barra", "Triceps", "Intermediario", "Tríceps testa" },
                    { 31, true, "Segurando um halter atrás da cabeça, estender os cotovelos para cima.", "Halteres", "Triceps", "Intermediario", "Tríceps francês" },
                    { 32, true, "Apoiado nas barras paralelas, descer o corpo flexionando os cotovelos e subir.", "Peso corporal", "Triceps", "Avancado", "Mergulho nas paralelas" },
                    { 33, true, "Deitado com joelhos flexionados, elevar o tronco contraindo o abdômen.", "Peso corporal", "Abdomen", "Iniciante", "Abdominal crunch" },
                    { 34, true, "Apoiado nos antebraços e pontas dos pés, manter o corpo alinhado pelo tempo definido.", "Peso corporal", "Abdomen", "Iniciante", "Prancha" },
                    { 35, true, "Deitado, elevar as pernas estendidas até 90 graus e descer sem encostar no chão.", "Peso corporal", "Abdomen", "Intermediario", "Elevação de pernas" },
                    { 36, true, "Ajoelhado de frente para a polia alta, flexionar o tronco puxando a corda.", "Polia", "Abdomen", "Intermediario", "Abdominal na polia" },
                    { 37, true, "Caminhada ou corrida com velocidade e inclinação ajustadas ao condicionamento.", "Esteira", "Cardio", "Iniciante", "Esteira" },
                    { 38, true, "Pedalar em ritmo constante ou intervalado, com carga ajustável.", "Bicicleta", "Cardio", "Iniciante", "Bicicleta ergométrica" },
                    { 39, true, "Remada completa empurrando com as pernas e puxando com os braços, em ritmo contínuo.", "Remo", "Cardio", "Intermediario", "Remo ergômetro" },
                    { 40, true, "Agachar, levar os pés para trás, fazer uma flexão, voltar e saltar, em sequência.", "Peso corporal", "Cardio", "Avancado", "Burpee" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercicios_GrupoMuscular",
                table: "Exercicios",
                column: "GrupoMuscular");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exercicios");
        }
    }
}
