using AvaliacoesService.DTO;
using AvaliacoesService.Infra;
using AvaliacoesService.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace AvaliacoesService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvaliacoesController : ControllerBase
    {
        private ServAvaliacoes _servAvaliacoes;

        public AvaliacoesController()
        {
            _servAvaliacoes = new ServAvaliacoes();
        }

        // Integração 3: valida o aluno em Alunos, salva a avaliação e atualiza o peso do aluno.
        [Route("/api/avaliacoes")]
        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] AvaliacaoCadastroDTO dto)
        {
            try
            {
                var avaliacaoDto = await _servAvaliacoes.Registrar(dto);

                return Created($"/api/avaliacoes/{avaliacaoDto.Id}", avaliacaoDto);
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

        [Route("/api/avaliacoes/{id}")]
        [HttpGet]
        public IActionResult Buscar(int id)
        {
            try
            {
                return Ok(_servAvaliacoes.Buscar(id));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("/api/avaliacoes/aluno/{alunoId}")]
        [HttpGet]
        public IActionResult ListarPorAluno(int alunoId)
        {
            try
            {
                return Ok(_servAvaliacoes.ListarPorAluno(alunoId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Reenvia ao serviço de Alunos as avaliações com Sincronizada = false.
        [Route("/api/avaliacoes/sincronizar")]
        [HttpPost]
        public async Task<IActionResult> Sincronizar()
        {
            try
            {
                return Ok(await _servAvaliacoes.SincronizarPendentes());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
