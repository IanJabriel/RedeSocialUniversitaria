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
        }

        public DbSet<Usuario> Usuarios { get; set; }
        //public DbSet<Postagem> Postagens { get; set; }
        //public DbSet<Evento> Eventos { get; set; }

    }
}
