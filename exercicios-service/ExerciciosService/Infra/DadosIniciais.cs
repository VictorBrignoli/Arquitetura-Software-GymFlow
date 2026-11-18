using ExerciciosService.Entidades;

namespace ExerciciosService.Infra
{
    // Catálogo inicial (seed) carregado pela migration. Grupos musculares sem acento para facilitar o filtro ?grupo=.
    public static class DadosIniciais
    {
        public static Exercicio[] Exercicios =>
        [
            // Peito
            new Exercicio { Id = 1, Nome = "Supino reto com barra", GrupoMuscular = "Peito", Equipamento = "Barra", NivelDificuldade = "Intermediario", Descricao = "Deitado no banco reto, descer a barra até o peito e empurrar até estender os braços." },
            new Exercicio { Id = 2, Nome = "Supino inclinado com halteres", GrupoMuscular = "Peito", Equipamento = "Halteres", NivelDificuldade = "Intermediario", Descricao = "No banco inclinado a 30-45 graus, empurrar os halteres para cima, focando na parte superior do peito." },
            new Exercicio { Id = 3, Nome = "Crucifixo com halteres", GrupoMuscular = "Peito", Equipamento = "Halteres", NivelDificuldade = "Iniciante", Descricao = "Deitado, abrir os braços com cotovelos levemente flexionados e fechar sobre o peito." },
            new Exercicio { Id = 4, Nome = "Crossover na polia", GrupoMuscular = "Peito", Equipamento = "Polia", NivelDificuldade = "Intermediario", Descricao = "De pé entre as polias, puxar os cabos à frente do corpo cruzando as mãos." },
            new Exercicio { Id = 5, Nome = "Flexão de braço", GrupoMuscular = "Peito", Equipamento = "Peso corporal", NivelDificuldade = "Iniciante", Descricao = "Com mãos no chão na largura dos ombros, descer o corpo alinhado e empurrar de volta." },

            // Costas
            new Exercicio { Id = 6, Nome = "Puxada frontal", GrupoMuscular = "Costas", Equipamento = "Polia", NivelDificuldade = "Iniciante", Descricao = "Sentado no pulley, puxar a barra até a altura do peito, aproximando as escápulas." },
            new Exercicio { Id = 7, Nome = "Remada curvada com barra", GrupoMuscular = "Costas", Equipamento = "Barra", NivelDificuldade = "Intermediario", Descricao = "Tronco inclinado à frente, puxar a barra em direção ao abdômen mantendo a coluna neutra." },
            new Exercicio { Id = 8, Nome = "Remada baixa na polia", GrupoMuscular = "Costas", Equipamento = "Polia", NivelDificuldade = "Iniciante", Descricao = "Sentado, puxar o triângulo em direção ao abdômen mantendo o tronco ereto." },
            new Exercicio { Id = 9, Nome = "Barra fixa", GrupoMuscular = "Costas", Equipamento = "Peso corporal", NivelDificuldade = "Avancado", Descricao = "Pendurado na barra, puxar o corpo até o queixo passar a barra." },
            new Exercicio { Id = 10, Nome = "Levantamento terra", GrupoMuscular = "Costas", Equipamento = "Barra", NivelDificuldade = "Avancado", Descricao = "Levantar a barra do chão estendendo quadril e joelhos, com a coluna neutra." },

            // Pernas
            new Exercicio { Id = 11, Nome = "Agachamento livre", GrupoMuscular = "Pernas", Equipamento = "Barra", NivelDificuldade = "Intermediario", Descricao = "Barra apoiada no trapézio, agachar até as coxas ficarem paralelas ao chão e subir." },
            new Exercicio { Id = 12, Nome = "Leg press 45", GrupoMuscular = "Pernas", Equipamento = "Máquina", NivelDificuldade = "Iniciante", Descricao = "Empurrar a plataforma com os pés afastados na largura do quadril, sem travar os joelhos." },
            new Exercicio { Id = 13, Nome = "Cadeira extensora", GrupoMuscular = "Pernas", Equipamento = "Máquina", NivelDificuldade = "Iniciante", Descricao = "Sentado, estender os joelhos até as pernas ficarem retas, isolando o quadríceps." },
            new Exercicio { Id = 14, Nome = "Mesa flexora", GrupoMuscular = "Pernas", Equipamento = "Máquina", NivelDificuldade = "Iniciante", Descricao = "Deitado de bruços, flexionar os joelhos levando os calcanhares em direção aos glúteos." },
            new Exercicio { Id = 15, Nome = "Afundo com halteres", GrupoMuscular = "Pernas", Equipamento = "Halteres", NivelDificuldade = "Intermediario", Descricao = "Dar um passo à frente e descer até o joelho de trás quase tocar o chão, alternando as pernas." },
            new Exercicio { Id = 16, Nome = "Panturrilha em pé", GrupoMuscular = "Pernas", Equipamento = "Máquina", NivelDificuldade = "Iniciante", Descricao = "Elevar os calcanhares o máximo possível e descer devagar, alongando a panturrilha." },

            // Glúteos
            new Exercicio { Id = 17, Nome = "Elevação pélvica com barra", GrupoMuscular = "Gluteos", Equipamento = "Barra", NivelDificuldade = "Intermediario", Descricao = "Costas apoiadas no banco, elevar o quadril com a barra sobre a pelve, contraindo os glúteos." },
            new Exercicio { Id = 18, Nome = "Glúteo na polia", GrupoMuscular = "Gluteos", Equipamento = "Polia", NivelDificuldade = "Iniciante", Descricao = "Com a caneleira presa à polia, estender o quadril levando a perna para trás." },
            new Exercicio { Id = 19, Nome = "Cadeira abdutora", GrupoMuscular = "Gluteos", Equipamento = "Máquina", NivelDificuldade = "Iniciante", Descricao = "Sentado, afastar as pernas contra a resistência da máquina." },

            // Ombros
            new Exercicio { Id = 20, Nome = "Desenvolvimento com halteres", GrupoMuscular = "Ombros", Equipamento = "Halteres", NivelDificuldade = "Iniciante", Descricao = "Sentado, empurrar os halteres acima da cabeça a partir da altura dos ombros." },
            new Exercicio { Id = 21, Nome = "Elevação lateral", GrupoMuscular = "Ombros", Equipamento = "Halteres", NivelDificuldade = "Iniciante", Descricao = "De pé, elevar os halteres lateralmente até a altura dos ombros." },
            new Exercicio { Id = 22, Nome = "Elevação frontal", GrupoMuscular = "Ombros", Equipamento = "Halteres", NivelDificuldade = "Iniciante", Descricao = "De pé, elevar os halteres à frente do corpo até a altura dos ombros." },
            new Exercicio { Id = 23, Nome = "Desenvolvimento militar com barra", GrupoMuscular = "Ombros", Equipamento = "Barra", NivelDificuldade = "Avancado", Descricao = "De pé, empurrar a barra da altura do peito até acima da cabeça, com o core firme." },
            new Exercicio { Id = 24, Nome = "Crucifixo inverso", GrupoMuscular = "Ombros", Equipamento = "Máquina", NivelDificuldade = "Iniciante", Descricao = "No peck deck de frente para o encosto, abrir os braços para trabalhar o deltoide posterior." },

            // Bíceps
            new Exercicio { Id = 25, Nome = "Rosca direta com barra", GrupoMuscular = "Biceps", Equipamento = "Barra", NivelDificuldade = "Iniciante", Descricao = "De pé, flexionar os cotovelos levando a barra até os ombros, sem balançar o tronco." },
            new Exercicio { Id = 26, Nome = "Rosca alternada", GrupoMuscular = "Biceps", Equipamento = "Halteres", NivelDificuldade = "Iniciante", Descricao = "Flexionar um braço de cada vez, girando o punho para cima durante a subida." },
            new Exercicio { Id = 27, Nome = "Rosca martelo", GrupoMuscular = "Biceps", Equipamento = "Halteres", NivelDificuldade = "Iniciante", Descricao = "Flexionar os cotovelos com a pegada neutra (palmas voltadas uma para a outra)." },
            new Exercicio { Id = 28, Nome = "Rosca Scott", GrupoMuscular = "Biceps", Equipamento = "Máquina", NivelDificuldade = "Intermediario", Descricao = "Com os braços apoiados no banco Scott, flexionar os cotovelos isolando o bíceps." },

            // Tríceps
            new Exercicio { Id = 29, Nome = "Tríceps pulley", GrupoMuscular = "Triceps", Equipamento = "Polia", NivelDificuldade = "Iniciante", Descricao = "De pé na polia alta, estender os cotovelos empurrando a barra para baixo." },
            new Exercicio { Id = 30, Nome = "Tríceps testa", GrupoMuscular = "Triceps", Equipamento = "Barra", NivelDificuldade = "Intermediario", Descricao = "Deitado, descer a barra em direção à testa flexionando apenas os cotovelos e estender." },
            new Exercicio { Id = 31, Nome = "Tríceps francês", GrupoMuscular = "Triceps", Equipamento = "Halteres", NivelDificuldade = "Intermediario", Descricao = "Segurando um halter atrás da cabeça, estender os cotovelos para cima." },
            new Exercicio { Id = 32, Nome = "Mergulho nas paralelas", GrupoMuscular = "Triceps", Equipamento = "Peso corporal", NivelDificuldade = "Avancado", Descricao = "Apoiado nas barras paralelas, descer o corpo flexionando os cotovelos e subir." },

            // Abdômen
            new Exercicio { Id = 33, Nome = "Abdominal crunch", GrupoMuscular = "Abdomen", Equipamento = "Peso corporal", NivelDificuldade = "Iniciante", Descricao = "Deitado com joelhos flexionados, elevar o tronco contraindo o abdômen." },
            new Exercicio { Id = 34, Nome = "Prancha", GrupoMuscular = "Abdomen", Equipamento = "Peso corporal", NivelDificuldade = "Iniciante", Descricao = "Apoiado nos antebraços e pontas dos pés, manter o corpo alinhado pelo tempo definido." },
            new Exercicio { Id = 35, Nome = "Elevação de pernas", GrupoMuscular = "Abdomen", Equipamento = "Peso corporal", NivelDificuldade = "Intermediario", Descricao = "Deitado, elevar as pernas estendidas até 90 graus e descer sem encostar no chão." },
            new Exercicio { Id = 36, Nome = "Abdominal na polia", GrupoMuscular = "Abdomen", Equipamento = "Polia", NivelDificuldade = "Intermediario", Descricao = "Ajoelhado de frente para a polia alta, flexionar o tronco puxando a corda." },

            // Cardio
            new Exercicio { Id = 37, Nome = "Esteira", GrupoMuscular = "Cardio", Equipamento = "Esteira", NivelDificuldade = "Iniciante", Descricao = "Caminhada ou corrida com velocidade e inclinação ajustadas ao condicionamento." },
            new Exercicio { Id = 38, Nome = "Bicicleta ergométrica", GrupoMuscular = "Cardio", Equipamento = "Bicicleta", NivelDificuldade = "Iniciante", Descricao = "Pedalar em ritmo constante ou intervalado, com carga ajustável." },
            new Exercicio { Id = 39, Nome = "Remo ergômetro", GrupoMuscular = "Cardio", Equipamento = "Remo", NivelDificuldade = "Intermediario", Descricao = "Remada completa empurrando com as pernas e puxando com os braços, em ritmo contínuo." },
            new Exercicio { Id = 40, Nome = "Burpee", GrupoMuscular = "Cardio", Equipamento = "Peso corporal", NivelDificuldade = "Avancado", Descricao = "Agachar, levar os pés para trás, fazer uma flexão, voltar e saltar, em sequência." },
        ];
    }
}
