using Microsoft.EntityFrameworkCore;
using RedeSocialUniversidade.Domain.Entities;

namespace RedeSocialUniversidade.Infra
{
    public class SqlContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RedeSocialUniversidade");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração para Seguidor
            modelBuilder.Entity<Seguidor>(entity =>
            {
                entity.HasKey(s => new { s.UsuarioSeguidoId, s.UsuarioSeguidorId });

                entity.HasOne(s => s.UsuarioSeguido)
                      .WithMany(u => u.Seguidores)
                      .HasForeignKey(s => s.UsuarioSeguidoId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.UsuarioSeguidor)
                      .WithMany()
                      .HasForeignKey(s => s.UsuarioSeguidorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração para Postagem
            modelBuilder.Entity<Postagem>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Conteudo).IsRequired().HasMaxLength(2000);
                entity.HasOne(p => p.Autor)
                      .WithMany()
                      .HasForeignKey(p => p.AutorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração para Comentario
            modelBuilder.Entity<Comentario>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Texto).IsRequired().HasMaxLength(500);
                entity.HasOne(c => c.Postagem)
                      .WithMany(p => p.Comentarios)
                      .HasForeignKey(c => c.PostagemId);
                entity.HasOne(c => c.Autor)
                      .WithMany()
                      .HasForeignKey(c => c.AutorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração para Curtida (tabela de junção)
            modelBuilder.Entity<Curtida>(entity =>
            {
                entity.HasKey(c => new { c.PostagemId, c.UsuarioId });
                entity.HasOne(c => c.Postagem)
                      .WithMany(p => p.Curtidas)
                      .HasForeignKey(c => c.PostagemId);
                entity.HasOne(c => c.Usuario)
                      .WithMany()
                      .HasForeignKey(c => c.UsuarioId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Confiugração para Evento
            modelBuilder.Entity<Evento>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Nome).IsRequired().HasMaxLength(100);
                e.Property(x => x.Local).IsRequired().HasMaxLength(200);
                e.HasMany(x => x.Inscricoes).WithOne(x => x.Evento);
            });

            modelBuilder.Entity<InscricaoEvento>(e =>
            {
                e.HasKey(x => new { x.EventoId, x.UsuarioId });
                e.HasOne(x => x.Usuario).WithMany().OnDelete(DeleteBehavior.Restrict);
            });
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Postagem> Postagens { get; set; }
        public DbSet<Evento> Eventos { get; set; }

    }
}
