using Microsoft.EntityFrameworkCore;
using RedeSocialUniversidade.Domain.Entities;
using RedeSocialUniversidade.Domain.Interface;

namespace RedeSocialUniversidade.Infra.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly SqlContext _context;

        public UsuarioRepository(SqlContext context)
        {
            _context = context;
        }

        public async Task<Usuario> ObterIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Seguidores)
                .ThenInclude(s => s.UsuarioSeguidor)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AdicionarAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Email == email);
        }
        public async Task AddAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}