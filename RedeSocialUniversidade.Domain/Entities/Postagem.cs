using RedeSocialUniversidade.Domain.Exceptions;

namespace RedeSocialUniversidade.Domain.Entities
{
    public class Postagem
    {
        public int Id { get; private set; }
        public int AutorId { get; private set; }
        public Usuario Autor { get; private set; }
        public string Conteudo { get; private set; }
        public DateTime DataHora { get; private set; } = DateTime.Now;

        private readonly List<Curtida> _curtidas = new();
        public IReadOnlyCollection<Curtida> Curtidas => _curtidas.AsReadOnly();

        private readonly List<Comentario> _comentarios = new();
        public IReadOnlyCollection<Comentario> Comentarios => _comentarios.AsReadOnly();

        protected Postagem() { } // Para o EF Core

        public Postagem(int autorId, string conteudo)
        {
            AutorId = autorId;
            Conteudo = conteudo;
            Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(Conteudo))
                throw new DomainException("Conteúdo da postagem não pode ser vazio");

            if (Conteudo.Length > 2000)
                throw new DomainException("Conteúdo muito longo (máximo 2000 caracteres)");
        }

        public void AdicionarCurtida(int postagemId ,int usuarioId)
        {
            if (_curtidas.Any(c => c.UsuarioId == usuarioId))
                throw new DomainException("Usuário já curtiu esta postagem");

            _curtidas.Add(new Curtida(postagemId, usuarioId));
        }

        public void AdicionarComentario(int autorId, string texto)
        {
            _comentarios.Add(new Comentario(this.Id, autorId, texto));
        }
    }

    public class Comentario
    {
        public int Id { get; private set; }
        public int PostagemId { get; private set; }
        public Postagem Postagem { get; private set; }
        public int AutorId { get; private set; }
        public Usuario Autor { get; private set; }
        public string Texto { get; private set; }
        public DateTime DataHora { get; private set; } = DateTime.Now;

        protected Comentario() { } // Para o EF Core

        public Comentario(int postagemId, int autorId, string texto)
        {
            PostagemId = postagemId;
            AutorId = autorId;
            Texto = texto;
            Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(Texto))
                throw new DomainException("Texto do comentário não pode ser vazio");

            if (Texto.Length > 500)
                throw new DomainException("Comentário muito longo (máximo 500 caracteres)");
        }
    }

    public class Curtida
    {
        public int PostagemId { get; private set; }
        public Postagem Postagem { get; private set; }
        public int UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }

        protected Curtida() { }

        public Curtida(int postagemId, int usuarioId)
        {
            PostagemId = postagemId;
            UsuarioId = usuarioId;
        }
    }
}