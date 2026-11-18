namespace AlunosService.DTO
{
    // Corpo do PATCH /api/alunos/{id}/medidas, chamado pelo serviço de Avaliações (integração 3).
    public class MedidasDTO
    {
        public double PesoAtual { get; set; }

        // Opcional: só altera a altura se for informada.
        public int? AlturaCm { get; set; }
    }
}
