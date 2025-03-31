namespace RedeSocialUniversidade.Domain.Entities
{
    public class Postagem
    {
        public int Id { get; set; }

        public Usuario Autor { get; set; }

        public string Conteudo { get; set; }

        public DateTime DataHora { get; set; }
        public List<Usuario> Curtidas { get; set;  }
        public List<Comentario> Comentarios { get; set; }

    }

    public class Comentario
    {
        public int Id { get; set; }
        public Usuario Autor { get; set; }
        public string Texto { get; set; }

    }
}
