namespace RedeSocialUniversidade.Application.DTOs.Usuario
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Curso { get; set; }
        public List<SeguidorResponseDto> Seguidores { get; set; } = new();
    }
}
