using RedeSocialUniversidade.Application.DTOs.Shared;

namespace RedeSocialUniversidade.Application.DTOs.Postagem
{
    public class PostagemResponseDto
    {
        public int Id { get; set; }
        public UsuarioResumidoDto Autor { get; set; }
        public string Conteudo { get; set; }
        public DateTime DataHora { get; set; }
        public int TotalCurtidas { get; set; }
        public List<ComentarioResponseDto> Comentarios { get; set; } = new();
    }
}
