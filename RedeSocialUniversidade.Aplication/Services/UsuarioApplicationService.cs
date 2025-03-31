using RedeSocialUniversidade.Application.DTOs;
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
            var usuario = await _usuarioRepo.ObterIdAsync(usuarioId)
                ?? throw new DomainException("Usuário não encontrado");

            var seguidor = await _usuarioRepo.ObterIdAsync(seguidorId)
                ?? throw new DomainException("Seguidor não encontrado");

            usuario.AdicionarSeguidor(seguidor.Id);

            if (!await _usuarioRepo.SaveChangesAsync())
            {
                throw new Exception("Falha ao salvar o seguidor");
            }
        }

        public async Task<Usuario> ObterUsuarioAsync(int id)
        {
            return await _usuarioRepo.ObterIdAsync(id)
                ?? throw new DomainException("Usuário não encontrado");
        }
    }
}