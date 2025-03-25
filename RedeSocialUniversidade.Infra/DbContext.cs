using Microsoft.EntityFrameworkCore;
using RedeSocialUniversidade.Domain.Entitys;

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


        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Postagem> Postagens { get; set; }
        public DbSet<Evento> Eventos { get; set; }

    }
}
