using RedeSocialUniversidade.Domain.Entities;

namespace RedeSocialUniversidade.Domain.Interface
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObterIdAsync(int id);
        Task AdicionarAsync(Usuario usuario);
        Task<bool> ExisteEmailAsync(string email);
        Task AddAsync(Usuario usuario);
        Task<bool> SaveChangesAsync();
    }
}