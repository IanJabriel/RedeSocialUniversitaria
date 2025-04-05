using System.ComponentModel.DataAnnotations;

namespace RedeSocialUniversidade.Application.DTOs.Usuario
{
    public record CriarUsuarioDto(
        [Required] string Nome,
        [EmailAddress] string Email,
        string Curso
        );
}