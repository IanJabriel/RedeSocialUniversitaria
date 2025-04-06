using Microsoft.EntityFrameworkCore;
using RedeSocialUniversidade.Domain.Entities;
using RedeSocialUniversidade.Domain.Interface;

namespace RedeSocialUniversidade.Infra.Repository
{
    public class PostagemRepository : IPostagemRepository
    {
        private readonly SqlContext _context;

        public PostagemRepository(SqlContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Postagem postagem)
        {
            await _context.Postagens.AddAsync(postagem);
        }

        public async Task<Postagem> ObterPorIdAsync(int id)
        {
            return await _context.Postagens
                .Include(p => p.Autor)
                .Include(p => p.Comentarios)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task SalvarAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Postagem> ObterComRelacionamentosAsync(int id)
        {
            return await _context.Postagens
                .Include(p => p.Autor)          // Carrega o autor da postagem
                .Include(p => p.Curtidas)       // Carrega a lista de curtidas
                    .ThenInclude(c => c.Usuario) // Carrega os usuários que curtiram
                .Include(p => p.Comentarios)    // Carrega os comentários
                    .ThenInclude(c => c.Autor)  // Carrega os autores dos comentários
                .AsNoTracking()                 // Melhora performance para consultas somente leitura
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Postagem> ObterComRelacionamentosParaEdicaoAsync(int id)
        {
            return await _context.Postagens
                .Include(p => p.Autor)
                .Include(p => p.Curtidas)
                    .ThenInclude(c => c.Usuario)
                .Include(p => p.Comentarios)
                    .ThenInclude(c => c.Autor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Postagem>> ListarPostagensPorUsuarioAsync(int usuarioId)
        {
            return await _context.Postagens
                .Where(p => p.AutorId == usuarioId)
                .Include(p => p.Autor)
                .Include(p => p.Comentarios)
                .ToListAsync();
        }
    }
}