using RedeSocialUniversidade.Domain.Entities;

namespace RedeSocialUniversidade.Domain.Interface
{
    public interface IPostagemRepository
    {
        Task AdicionarAsync(Postagem postagem);
        Task<Postagem> ObterPorIdAsync(int id);
        Task<Postagem> ObterComRelacionamentosAsync(int id);
        Task<List<Postagem>> ListarPostagensPorUsuarioAsync(int id);
        Task SalvarAsync();
    }
}