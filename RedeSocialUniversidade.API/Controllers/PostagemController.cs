using Microsoft.AspNetCore.Mvc;
using RedeSocialUniversidade.Application.DTOs.Postagem;
using RedeSocialUniversidade.Application.Services;
using RedeSocialUniversidade.Domain.Exceptions;

namespace RedeSocialUniversidade.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostagensController : ControllerBase
    {
        private readonly PostagemAppService _postagemService;

        public PostagensController(PostagemAppService postagemService)
        {
            _postagemService = postagemService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarPostagem([FromBody] CriarPostagemDto dto)
        {
            try
            {
                var postagem = await _postagemService.CriarPostagemAsync(dto);
                return CreatedAtAction(nameof(ObterPostagem), new { id = postagem.Id }, postagem);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPostagem(int id)
        {
            try
            {
                var postagem = await _postagemService.ObterPostagemAsync(id);
                return Ok(postagem);
            }
            catch (DomainException ex)
            {
                return NotFound(new { ex.Message });
            }
        }

        [HttpPost("{postagemId}/curtir")]
        public async Task<IActionResult> CurtirPostagem([FromRoute] int postagemId, [FromBody] CurtirPostagemDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _postagemService.CurtirPostagemAsync(postagemId, dto);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erro interno ao curtir postagem" });
            }
        }

        [HttpPost("{postagemId}/comentarios")]
        public async Task<IActionResult> AdicionarComentario(int postagemId, [FromBody] ComentarioRequestDto dto)
        {
            try
            {
                await _postagemService.AdicionarComentarioAsync(postagemId, dto);
                return NoContent(); // Retorna 204 No Content para sucesso
            }
            catch (DomainException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Erro interno ao adicionar comentário" });
            }
        }

    }
}