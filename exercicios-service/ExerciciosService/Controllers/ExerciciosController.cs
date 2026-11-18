using ExerciciosService.DTO;
using ExerciciosService.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace ExerciciosService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExerciciosController : ControllerBase
    {
        private ServExercicios _servExercicios;

        public ExerciciosController()
        {
            _servExercicios = new ServExercicios();
        }

        // Aceita filtros opcionais: /api/exercicios?grupo=Peito&ativo=true
        [Route("/api/exercicios")]
        [HttpGet]
        public IActionResult Listar([FromQuery] string? grupo, [FromQuery] bool? ativo)
        {
            try
            {
                return Ok(_servExercicios.Listar(grupo, ativo));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Usado pela integração 2 (Treinos valida e enriquece os itens do treino).
        [Route("/api/exercicios/{id}")]
        [HttpGet]
        public IActionResult Buscar(int id)
        {
            try
            {
                return Ok(_servExercicios.Buscar(id));
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

        [Route("/api/exercicios")]
        [HttpPost]
        public IActionResult Cadastrar([FromBody] ExercicioCadastroDTO dto)
        {
            try
            {
                var exercicioDto = _servExercicios.Cadastrar(dto);

                return Created($"/api/exercicios/{exercicioDto.Id}", exercicioDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("/api/exercicios/{id}")]
        [HttpPut]
        public IActionResult Atualizar(int id, [FromBody] ExercicioCadastroDTO dto)
        {
            try
            {
                return Ok(_servExercicios.Atualizar(id, dto));
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
