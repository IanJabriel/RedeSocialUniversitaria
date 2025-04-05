using RedeSocialUniversidade.Application.DTOs.Shared;

namespace RedeSocialUniversidade.Application.DTOs.Postagem
{
    public class ComentarioResponseDto
    {
        public int Id { get; set; }
        public UsuarioResumidoDto Autor { get; set; }
        public string Texto { get; set; }
        public DateTime DataHora { get; set; }
    }
}
