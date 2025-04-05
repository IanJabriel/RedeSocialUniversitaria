using RedeSocialUniversidade.Application.DTOs.Evento;
using RedeSocialUniversidade.Domain.Entities;
using RedeSocialUniversidade.Domain.Exceptions;
using RedeSocialUniversidade.Domain.Interface;
using System.Data;

public class EventoAppService
{
    private readonly IEventoRepository _eventoRepo;
    private readonly IUsuarioRepository _usuarioRepo;

    public EventoAppService(
        IEventoRepository eventoRepo, IUsuarioRepository usuarioRepo) 
    {
        _eventoRepo = eventoRepo;
        _usuarioRepo = usuarioRepo;
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

        await _eventoRepo.AdicionarAsync(evento);
        await _eventoRepo.SalvarAsync();

        return evento;
    }

    public async Task RegistrarInscricaoAsync(int eventoId, string usuarioEmail)
    {
        var usuario = await _usuarioRepo.ObterPorEmailAsync(usuarioEmail);
        DomainException.When(usuario == null, "Usuário não encontrado");

        await _eventoRepo.RegistrarInscricaoAsync(eventoId, usuario.Email);
    }
}