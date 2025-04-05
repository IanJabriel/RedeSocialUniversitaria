using System.ComponentModel.DataAnnotations;

namespace RedeSocialUniversidade.Application.DTOs.Evento
{
    public class InscricaoEventoDto
    {
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email em formato inválido")]
        public string Email { get; set; }
    }
}