using RedeSocialUniversidade.Domain.Entities;

namespace RedeSocialUniversidade.Domain.Interface
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObterIdAsync(int id);
        Task<bool> ExisteIdAsync(int id);
        Task<Usuario> ObterPorIdAsync(int id);
        Task AdicionarAsync(Usuario usuario);
        Task<bool> ExisteEmailAsync(string email);
        Task AddAsync(Usuario usuario);
        Task<bool> SaveChangesAsync();
        Task<Usuario> ObterComSeguidoresAsync(int id);
        Task<Usuario> ObterPorEmailAsync(string email);
        Task<bool> UsuarioSegueOutroAsync(int usuarioId, int seguidorId);
    }
}