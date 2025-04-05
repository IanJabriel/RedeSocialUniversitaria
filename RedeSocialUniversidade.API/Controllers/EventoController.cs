using Microsoft.AspNetCore.Mvc;
using RedeSocialUniversidade.Application.DTOs.Evento;   
using RedeSocialUniversidade.Domain.Entities;
using RedeSocialUniversidade.Domain.Exceptions;
using RedeSocialUniversidade.Domain.Interface;
using RedeSocialUniversidade.Domain.Services;
using System.Security.Claims;


namespace RedeSocialUniversidade.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public EventosController(IEventoRepository eventoRepository, IUsuarioRepository usurarioRepository)
        {
            _eventoRepository = eventoRepository;
            _usuarioRepository = usurarioRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CriarEvento([FromBody] CriarEventoDto dto)
        {
            try
            {
                var evento = new Evento(
                    dto.Nome,
                    dto.Local,
                    dto.Descricao,
                    dto.DataHora,
                    dto.ExigeInscricao,
                    dto.LimiteParticipantes);

                await _eventoRepository.AdicionarAsync(evento);
                await _eventoRepository.SalvarAsync();

                return CreatedAtAction(nameof(ObterEventoPorId), new { id = evento.Id }, evento);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erro interno ao criar evento", Detalhes = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterEventoPorId(int id)
        {
            try
            {
                var evento = await _eventoRepository.ObterPorIdAsync(id);

                if (evento == null)
                    return NotFound(new { Message = "Evento não encontrado" });

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erro interno ao obter evento", Detalhes = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodosEventos()
        {
            try
            {
                var eventos = await _eventoRepository.ObterTodosAsync();
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erro interno ao listar eventos", Detalhes = ex.Message });
            }
        }

        [HttpPost("{eventoId}/inscricoes")]
        public async Task<IActionResult> AdicionarInscricao(int eventoId, [FromBody] InscricaoEventoDto dto)
        {
            try
            {
                await _eventoRepository.RegistrarInscricaoAsync(eventoId, dto.Email);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Erro ao processar inscrição",
                    Detalhes = ex.Message
                });
            }
        }

        public record InscricaoEvento(string Email);

        [HttpDelete("{eventoId}/inscricoes")]
        public async Task<IActionResult> CancelarInscricao(int eventoId)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}