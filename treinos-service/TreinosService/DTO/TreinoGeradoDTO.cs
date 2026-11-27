namespace TreinosService.DTO
{
    // Resposta do POST /api/treinos/gerar-ia
    public class TreinoGeradoDTO
    {
        public bool GeradoPorIa { get; set; }   // false = caiu no treino-modelo
        public TreinoDTO Treino { get; set; } = new();
    }
}