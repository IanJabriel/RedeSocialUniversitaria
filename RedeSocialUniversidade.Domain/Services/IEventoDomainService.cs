using RedeSocialUniversidade.Domain.Entities;
using RedeSocialUniversidade.Domain.Exceptions;
using RedeSocialUniversidade.Domain.Interface;
using System;
using System.Threading.Tasks;

namespace RedeSocialUniversidade.Domain.Services
{
    public interface IEventoDomainService
    {
        Task ValidarCriacaoEvento(Evento evento);
        Task ValidarInscricaoEvento(Usuario usuario, Evento evento);
    }

    public class EventoDomainService : IEventoDomainService
    {
        private readonly IEventoRepository _eventoRepository;

        public EventoDomainService(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }

        public async Task ValidarCriacaoEvento(Evento evento)
        {
            if (evento == null)
                throw new DomainException("Evento inválido");

            if (string.IsNullOrWhiteSpace(evento.Nome))
                throw new DomainException("Nome do evento não pode ser vazio");

            if (evento.Nome.Length > 100)
                throw new DomainException("Nome do evento excede o limite de 100 caracteres");

            var eventoConflitante = await _eventoRepository
                .ObterEventoNoPeriodo(evento.Local, evento.DataHora, evento.DataHora);

            if (eventoConflitante != null)
                throw new DomainException($"Já existe um evento agendado neste local: {eventoConflitante.Nome}");
        }

        public async Task ValidarInscricaoEvento(Usuario usuario, Evento evento)
        {
            if (usuario == null)
                throw new DomainException("Usuário inválido");

            if (evento == null)
                throw new DomainException("Evento inválido");

            if (!evento.ExigeInscricao)
                throw new DomainException("Este evento não requer inscrição");

            if (evento.DataHora < DateTime.Now)
                throw new DomainException("Evento já ocorreu");

            if (await _eventoRepository.UsuarioEstaInscritoAsync(evento.Id, usuario.Id))
                throw new DomainException("Usuário já está inscrito neste evento");

            if(evento.LimiteParticipantes > 0)
            {
                var totalInscritos = await _eventoRepository.ContarInscricoesAsync(evento.Id);
                if (totalInscritos >= evento.LimiteParticipantes) throw new DomainException("O Evento atingiu o limite de participantes");
            }
        }
    }
}