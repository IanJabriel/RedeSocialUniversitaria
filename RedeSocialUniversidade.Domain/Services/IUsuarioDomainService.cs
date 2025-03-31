using RedeSocialUniversidade.Domain.Entities;
using RedeSocialUniversidade.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace RedeSocialUniversidade.Domain.Services
{
    public interface IUsuarioDomainService
    {
        void AdicionarSeguidor(Usuario usuario, Usuario seguidor);
        void ValidarUsuario(Usuario usuario);
    }

    public class UsuarioDomainService : IUsuarioDomainService
    {

        public UsuarioDomainService()
        {
        }

        public void AdicionarSeguidor(Usuario usuario, Usuario seguidor)
        {
            if (usuario == null || seguidor == null)
                throw new DomainException("Usuário e seguidor são obrigatórios");

            if (usuario.Id == seguidor.Id)
                throw new DomainException("Um usuário não pode seguir a si mesmo");

            if (usuario.Seguidores.Any(s => s.UsuarioSeguidorId == seguidor.Id))
                throw new DomainException("Este usuário já está sendo seguido");

            usuario.AdicionarSeguidor(seguidor.Id);
        }

        public void ValidarUsuario(Usuario usuario)
        {
            if (usuario == null)
                throw new DomainException("Usuário inválido");

            if (string.IsNullOrWhiteSpace(usuario.Nome))
                throw new DomainException("Nome é obrigatório");

            if (string.IsNullOrWhiteSpace(usuario.Email))
                throw new DomainException("Email é obrigatório");

            if (!new EmailAddressAttribute().IsValid(usuario.Email))
                throw new DomainException("Email inválido");
        }
    }
}