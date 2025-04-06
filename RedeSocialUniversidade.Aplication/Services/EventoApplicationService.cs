using RedeSocialUniversidade.Application.DTOs.Evento;
using RedeSocialUniversidade.Domain.Entities;
using RedeSocialUniversidade.Domain.Exceptions;
using RedeSocialUniversidade.Domain.Interface;
using RedeSocialUniversidade.Domain.Services;

public class EventoAppService
{
    private readonly IEventoRepository _eventoRepo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IEventoDomainService _eventoDomainService;

    public EventoAppService(
        IEventoRepository eventoRepo,
        IUsuarioRepository usuarioRepo,
        IEventoDomainService eventoDomainService)
    {
        _eventoRepo = eventoRepo;
        _usuarioRepo = usuarioRepo;
        _eventoDomainService = eventoDomainService;
    }

    public async Task<Evento> CriarEventoAsync(CriarEventoDto dto)
    {
        var evento = new Evento(
            dto.Nome,
            dto.Local,
            dto.Descricao,
            dto.DataHora,
            dto.ExigeInscricao,
            dto.LimiteParticipantes);

        // Validações de domínio
        await _eventoDomainService.ValidarCriacaoEvento(evento);

        await _eventoRepo.AdicionarAsync(evento);
        await _eventoRepo.SalvarAsync();

        return evento;
    }

    public async Task RegistrarInscricaoAsync(int eventoId, string usuarioEmail)
    {
        var usuario = await _usuarioRepo.ObterPorEmailAsync(usuarioEmail);
        DomainException.When(usuario == null, "Usuário não encontrado");

        var evento = await _eventoRepo.ObterComInscricoesAsync(eventoId);
        DomainException.When(evento == null, "Evento não encontrado");

        // Validações de domínio para inscrição
        await _eventoDomainService.ValidarInscricaoEvento(usuario, evento);

        await _eventoRepo.RegistrarInscricaoAsync(eventoId, usuario.Email);
    }
}