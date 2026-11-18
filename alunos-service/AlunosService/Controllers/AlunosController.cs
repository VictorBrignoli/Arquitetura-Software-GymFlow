using AlunosService.DTO;
using AlunosService.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace AlunosService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlunosController : ControllerBase
    {
        private ServAlunos _servAlunos;

        public AlunosController()
        {
            _servAlunos = new ServAlunos();
        }

        [Route("/api/alunos")]
        [HttpGet]
        public IActionResult Listar([FromQuery] bool? ativo)
        {
            try
            {
                return Ok(_servAlunos.Listar(ativo));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Usado pela integração 1 (Treinos) e pela validação da integração 3 (Avaliações).
        [Route("/api/alunos/{id}")]
        [HttpGet]
        public IActionResult Buscar(int id)
        {
            try
            {
                return Ok(_servAlunos.Buscar(id));
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

        [Route("/api/alunos")]
        [HttpPost]
        public IActionResult Cadastrar([FromBody] AlunoCadastroDTO dto)
        {
            try
            {
                var alunoDto = _servAlunos.Cadastrar(dto);

                return Created($"/api/alunos/{alunoDto.Id}", alunoDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("/api/alunos/{id}")]
        [HttpPut]
        public IActionResult Atualizar(int id, [FromBody] AlunoCadastroDTO dto)
        {
            try
            {
                return Ok(_servAlunos.Atualizar(id, dto));
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

        // Integração 3: chamado pelo serviço de Avaliações depois de registrar uma avaliação física.
        [Route("/api/alunos/{id}/medidas")]
        [HttpPatch]
        public IActionResult AtualizarMedidas(int id, [FromBody] MedidasDTO dto)
        {
            try
            {
                return Ok(_servAlunos.AtualizarMedidas(id, dto));
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
    }
}
