using System.ComponentModel.DataAnnotations;

namespace RedeSocialUniversidade.Application.DTOs
{
    public record CriarUsuarioDto(
        [Required] string Nome,
        [EmailAddress] string Email,
        string Curso
        );
}