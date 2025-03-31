using RedeSocialUniversidade.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace RedeSocialUniversidade.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Curso { get; private set; }

        private readonly List<Seguidor> _seguidores = new();
        public IReadOnlyCollection<Seguidor> Seguidores => _seguidores.AsReadOnly();

        protected Usuario() { }

        public Usuario(string nome, string email, string curso)
        {
            Nome = nome;
            Email = email;
            Curso = curso;
            Validar();
        }

        public void AdicionarSeguidor(int usuarioSeguidorId)
        {
            if (usuarioSeguidorId == Id) throw new DomainException("Não pode seguir a si mesmo");
            if (!_seguidores.Any(s => s.UsuarioSeguidorId == usuarioSeguidorId))
            {
                _seguidores.Add(new Seguidor(Id, usuarioSeguidorId));
            }
        }

        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(Nome)) throw new DomainException("Nome inválido");
            if (!new EmailAddressAttribute().IsValid(Email)) throw new DomainException("Email inválido");
        }
    }

    public class Seguidor
    {
        public int UsuarioSeguidoId { get; private set; }
        public Usuario UsuarioSeguido { get; private set; }
        public int UsuarioSeguidorId { get; private set; }
        public Usuario UsuarioSeguidor { get; private set; }

        internal Seguidor(int usuarioSeguidoId, int usuarioSeguidorId)
        {
            UsuarioSeguidoId = usuarioSeguidoId;
            UsuarioSeguidorId = usuarioSeguidorId;
        }
    }
}