using RedeSocialUniversidade.Domain.Exceptions;

namespace RedeSocialUniversidade.Domain.Entities
{
    public class Evento
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Local { get; private set; }
        public string Descricao { get; private set; }
        public DateTime DataHora { get; private set; }
        public bool ExigeInscricao { get; private set; }
        public int LimiteParticipantes { get; private set; }

        private readonly List<InscricaoEvento> _inscricoes = new();
        public IReadOnlyCollection<InscricaoEvento> Inscricoes => _inscricoes.AsReadOnly();

        public Evento(string nome, string local, string descricao, DateTime dataHora,
                     bool exigeInscricao = false, int limiteParticipantes = 0)
        {
            Nome = nome;
            Local = local;
            Descricao = descricao;
            DataHora = dataHora;
            ExigeInscricao = exigeInscricao;
            LimiteParticipantes = limiteParticipantes;
            Validar();
        }

        // Métodos de domínio
        public void Participar(Usuario participante)
        {
            if (!ExigeInscricao)
                return; // Evento aberto, não precisa de inscrição

            if (LimiteParticipantes > 0 && _inscricoes.Count >= LimiteParticipantes)
                throw new DomainException("Evento atingiu o limite de participantes");

            if (_inscricoes.Any(i => i.UsuarioId == participante.Id))
                throw new DomainException("Usuário já está inscrito neste evento");

            _inscricoes.Add(new InscricaoEvento(Id, participante.Id));
        }

        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(Nome))
                throw new DomainException("Nome do evento é obrigatório");

            //if (DataHora <= DateTime.Now)
            //    throw new DomainException("Evento deve ser agendado para o futuro");
        }
    }

    public class InscricaoEvento
    {
        public int EventoId { get; private set; }
        public Evento Evento { get; private set; }
        public int UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }
        public DateTime DataInscricao { get; private set; } = DateTime.Now;

        public InscricaoEvento(int eventoId, int usuarioId)
        {
            EventoId = eventoId;
            UsuarioId = usuarioId;
        }
    }
}