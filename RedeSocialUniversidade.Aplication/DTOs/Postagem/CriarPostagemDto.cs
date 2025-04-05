using System.ComponentModel.DataAnnotations;

namespace RedeSocialUniversidade.Application.DTOs.Postagem
{
    public class CriarPostagemDto
    {
        [Required]
        public int AutorId { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 1)]
        public string Conteudo { get; set; }
    }
}
