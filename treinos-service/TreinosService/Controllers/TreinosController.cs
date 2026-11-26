using Microsoft.AspNetCore.Mvc;
using TreinosService.DTO;
using TreinosService.Infra;
using TreinosService.Servicos;

namespace TreinosService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TreinosController : ControllerBase
    {
        private ServTreino _servTreino;

        public TreinosController()
        {
            _servTreino = new ServTreino();
        }

        // Integração 1: valida o aluno no serviço de Alunos.
        [Route("/api/treinos")]
        [HttpPost]
        public Task<IActionResult> Criar([FromBody] TreinoCadastroDTO dto) => Tratar(async () =>
        {
            var treino = await _servTreino.Criar(dto);
            return Created($"/api/treinos/{treino.Id}", treino);
        });

        [Route("/api/treinos/{id}")]
        [HttpGet]
        public Task<IActionResult> Buscar(int id) => Tratar(async () =>
            Ok(await _servTreino.Buscar(id)));

        [Route("/api/treinos/aluno/{alunoId}")]
        [HttpGet]
        public Task<IActionResult> ListarPorAluno(int alunoId) => Tratar(async () =>
            Ok(await _servTreino.ListarPorAluno(alunoId)));

        // Integração 2: valida o exercício no serviço de Exercícios.
        [Route("/api/treinos/{id}/itens")]
        [HttpPost]
        public Task<IActionResult> AdicionarItem(int id, [FromBody] TreinoItemCadastroDTO dto) => Tratar(async () =>
        {
            var item = await _servTreino.AdicionarItem(id, dto);
            return Created($"/api/treinos/{id}", item);
        });

        [Route("/api/treinos/{id}/itens/{itemId}")]
        [HttpDelete]
        public Task<IActionResult> RemoverItem(int id, int itemId) => Tratar(() =>
        {
            _servTreino.RemoverItem(id, itemId);
            return Task.FromResult<IActionResult>(NoContent());
        });

        //[Route("/api/treinos/gerar-ia")]
        //[HttpPost]
        //public Task<IActionResult> GerarComIa([FromBody] GerarTreinoIaDTO dto) => Tratar(async () =>
        //{
        //    var gerado = await _servTreino.GerarComIa(dto);
        //    return Created($"/api/treinos/{gerado.Treino.Id}", gerado);
        //});

        // Mesmo mapeamento dos outros serviços: 404 / 503 / 400.
        private async Task<IActionResult> Tratar(Func<Task<IActionResult>> acao)
        {
            try
            {
                return await acao();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ServicoIndisponivelException e)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
      