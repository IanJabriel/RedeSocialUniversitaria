using RedeSocialUniversidade.Domain.Entities;

namespace RedeSocialUniversidade.Domain.Interface
{
    public interface IEventoRepository
    {
        Task AdicionarAsync(Evento evento);
        Task<Evento> ObterPorIdAsync(int id);
        Task<Evento> ObterComInscricoesAsync(int id);
        Task<IEnumerable<Evento>> ObterTodosAsync();
        Task<bool> ExisteAsync(int id);
        Task SalvarAsync();
        Task RegistrarInscricaoAsync(int eventoId, string usuarioEmail);
        Task<bool> UsuarioEstaInscritoAsync(int eventoId, int usuarioId);
        Task<Evento> ObterEventoNoPeriodo(string local, DateTime inicio, DateTime fim);
    }
}