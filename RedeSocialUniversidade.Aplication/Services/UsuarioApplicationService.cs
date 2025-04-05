using RedeSocialUniversidade.Application.DTOs.Usuario;
using RedeSocialUniversidade.Domain.Entities;
using RedeSocialUniversidade.Domain.Exceptions;
using RedeSocialUniversidade.Domain.Interface;

namespace RedeSocialUniversidade.Application.Services
{
    public class UsuarioAppService
    {
        private readonly IUsuarioRepository _usuarioRepo;

        public UsuarioAppService(IUsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        public async Task CriarUsuarioAsync(CriarUsuarioDto dto)
        {
            var usuario = new Usuario(dto.Nome, dto.Email, dto.Curso);
            await _usuarioRepo.AddAsync(usuario);

            if (!await _usuarioRepo.SaveChangesAsync())
            {
                throw new Exception("Falha ao salvar usuário");
            }
        }

        public async Task SeguirUsuarioAsync(int usuarioId, int seguidorId)
        {
            if (usuarioId == seguidorId)
                throw new DomainException("Não pode seguir a si mesmo");

            var usuario = await _usuarioRepo.ObterComSeguidoresAsync(usuarioId)
                ?? throw new DomainException("Usuário não encontrado");

            if (await _usuarioRepo.UsuarioSegueOutroAsync(usuarioId, seguidorId))
                throw new DomainException("Você já segue este usuário");

            var seguidor = await _usuarioRepo.ObterIdAsync(seguidorId)
                ?? throw new DomainException("Seguidor não encontrado");

            usuario.AdicionarSeguidor(seguidor.Id);

            if (!await _usuarioRepo.SaveChangesAsync())
                throw new Exception("Falha ao salvar o seguidor");
        }

        public async Task<Usuario> ObterUsuarioAsync(int id)
        {
            return await _usuarioRepo.ObterIdAsync(id)
                ?? throw new DomainException("Usuário não encontrado");
        }
    }
}