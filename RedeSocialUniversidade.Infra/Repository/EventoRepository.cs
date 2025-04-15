using Microsoft.EntityFrameworkCore;
using RedeSocialUniversidade.Domain.Entities;
using RedeSocialUniversidade.Domain.Exceptions;
using RedeSocialUniversidade.Domain.Interface;

namespace RedeSocialUniversidade.Infra.Repository
{
    public class EventoRepository : IEventoRepository
    {
        private readonly SqlContext _context;

        public EventoRepository(SqlContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Evento evento)
        {
            if (evento == null)
                throw new ArgumentNullException(nameof(evento));

            await _context.Eventos.AddAsync(evento);
        }
        public async Task RemoverAsync(Evento evento)
        {
            _context.Eventos.Remove(evento);
        }

        public async Task<Evento> ObterPorIdAsync(int id)
        {
            return await _context.Eventos
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Evento> ObterComInscricoesAsync(int id)
        {
            return await _context.Eventos
                .Include(e => e.Inscricoes)
                    .ThenInclude(i => i.Usuario)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Evento>> ObterTodosAsync()
        {
            return await _context.Eventos
                .AsNoTracking()
                .OrderBy(e => e.Id)
                .ToListAsync();
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Eventos
                .AsNoTracking()
                .AnyAsync(e => e.Id == id);
        }

        public async Task SalvarAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RegistrarInscricaoAsync(int eventoId, string usuarioEmail)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == usuarioEmail);

            DomainException.When(usuario == null, "Usuário não encontrado");

            var evento = await _context.Eventos
                .FirstOrDefaultAsync(e => e.Id == eventoId);

            DomainException.When(evento == null, "Evento não encontrado");

            bool jaInscrito = await _context.Set<InscricaoEvento>()
                .AnyAsync(i => i.EventoId == eventoId && i.UsuarioId == usuario.Id);

            DomainException.When(jaInscrito, "Usuário já está inscrito neste evento");

            try
            {
                var inscricao = new InscricaoEvento(eventoId, usuario.Id);
                await _context.AddAsync(inscricao);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DomainException("Erro ao registrar inscrição no banco de dados");
            }
        }

        public async Task<bool> UsuarioEstaInscritoAsync(int eventoId, int usuarioId)
        {
            return await _context.Set<InscricaoEvento>()
                .AnyAsync(i => i.EventoId == eventoId && i.UsuarioId == usuarioId);
        }

        public async Task<Evento> ObterEventoNoPeriodo(string local, DateTime inicio, DateTime fim)
        {
            return await _context.Eventos
                .AsNoTracking()
                .FirstOrDefaultAsync(e =>
                    e.Local == local &&
                    e.DataHora >= inicio &&
                    e.DataHora <= fim);
        }

        public async Task<int> ContarInscricoesAsync(int eventoId)
        {
            return await _context.Set<InscricaoEvento>()
            .CountAsync(i => i.EventoId == eventoId);
        }

        public async Task CancelarInscricaoAsync(int eventoId, int usuarioId)
        {
            var inscricao = await _context.Set<InscricaoEvento>()
                .FirstOrDefaultAsync(i => i.EventoId == eventoId && i.UsuarioId == usuarioId);

            if (inscricao == null)
                throw new DomainException("Inscrição não encontrada");

            _context.Set<InscricaoEvento>().Remove(inscricao);
            await _context.SaveChangesAsync();
        }

    }
}