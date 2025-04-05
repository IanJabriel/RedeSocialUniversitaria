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

        public async Task<bool> ExisteIdAsync(int id)
        {
            return await _context.Usuarios.AnyAsync(u => u.Id == id);
        }

        public async Task AdicionarAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
        }

        public async Task<Usuario> ObterComSeguidoresAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Seguidores)
                    .ThenInclude(s => s.UsuarioSeguidor)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> UsuarioSegueOutroAsync(int usuarioId, int seguidorId)
        {
            return await _context.Set<Seguidor>()
                .AnyAsync(s => s.UsuarioSeguidoId == usuarioId &&
                              s.UsuarioSeguidorId == seguidorId);
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

        public async Task<Usuario> ObterPorIdAsync(int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<Usuario> ObterPorEmailAsync(string email)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}