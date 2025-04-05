using System.ComponentModel.DataAnnotations;

namespace RedeSocialUniversidade.Application.DTOs.Evento
{
    public class CriarEventoDto
    {
        [Required, MaxLength(100)]
        public string Nome { get; set; }

        [Required, MaxLength(200)]
        public string Local { get; set; }

        public string Descricao { get; set; }

        [Required]
        public DateTime DataHora { get; set; }

        public bool ExigeInscricao { get; set; }

        [Range(0, int.MaxValue)]
        public int LimiteParticipantes { get; set; }
    }
}