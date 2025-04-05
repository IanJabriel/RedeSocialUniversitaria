using Microsoft.AspNetCore.Mvc;
using RedeSocialUniversidade.Application.DTOs.Usuario;
using RedeSocialUniversidade.Application.Services;
using RedeSocialUniversidade.Domain.Exceptions;

namespace RedeSocialUniversidade.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioAppService _appService;

        public UsuariosController(UsuarioAppService appService)
        {
            _appService = appService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioDto dto)
        {
            try
            {
                await _appService.CriarUsuarioAsync(dto);
                return Ok(new { Message = "Usuário criado com sucesso" });
            }
            catch (DomainException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Erro interno no servidor" });
            }
        }

        [HttpPost("{usuarioId}/seguir/{seguidorId}")]
        public async Task<IActionResult> SeguirUsuario(int usuarioId, int seguidorId)
        {
            try
            {
                await _appService.SeguirUsuarioAsync(usuarioId, seguidorId);
                return Ok(new { Message = "Usuário seguido com sucesso" });
            }
            catch (DomainException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Erro ao seguir usuário" });
            }
        }   

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterUsuario(int id)
        {
            try
            {
                var usuario = await _appService.ObterUsuarioAsync(id);

                var response = new UsuarioResponseDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Curso = usuario.Curso,
                    Seguidores = usuario.Seguidores.Select(s => new SeguidorResponseDto
                    {
                        Id = s.UsuarioSeguidorId,
                        Nome = s.UsuarioSeguidor?.Nome ?? "Não disponível",
                    }).ToList()
                };

                return Ok(response);
            }
            catch (DomainException ex)
            {
                return NotFound(new { ex.Message });
            }
        }
    }
}