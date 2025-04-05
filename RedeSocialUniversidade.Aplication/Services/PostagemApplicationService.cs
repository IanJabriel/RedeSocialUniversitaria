using RedeSocialUniversidade.Application.DTOs.Postagem;
using RedeSocialUniversidade.Application.DTOs.Shared;
using RedeSocialUniversidade.Domain.Entities;
using RedeSocialUniversidade.Domain.Exceptions;
using RedeSocialUniversidade.Domain.Interface;

namespace RedeSocialUniversidade.Application.Services
{
    public class PostagemAppService
    {
        private readonly IPostagemRepository _postagemRepo;
        private readonly IUsuarioRepository _usuarioRepo;

        public PostagemAppService(IPostagemRepository postagemRepo, IUsuarioRepository usuarioRepo)
        {
            _postagemRepo = postagemRepo;
            _usuarioRepo = usuarioRepo;
        }

        public async Task<PostagemResponseDto> CriarPostagemAsync(CriarPostagemDto dto)
        {
            var usuario = await _usuarioRepo.ObterIdAsync(dto.AutorId);
            if (usuario == null)
                throw new DomainException("Usuário não encontrado");

            var postagem = new Postagem(dto.AutorId, dto.Conteudo);
            await _postagemRepo.AdicionarAsync(postagem);
            await _postagemRepo.SalvarAsync();

            var postagemComAutor = await _postagemRepo.ObterComRelacionamentosAsync(postagem.Id);
            return MapearParaDto(postagemComAutor);
        }

        public async Task<PostagemResponseDto> ObterPostagemAsync(int id)
        {
            var postagem = await _postagemRepo.ObterComRelacionamentosAsync(id);
            if (postagem == null)
                throw new DomainException("Postagem não encontrada");

            return MapearParaDto(postagem);
        }

        public async Task CurtirPostagemAsync(int postagemId, CurtirPostagemDto dto)
        {
            var postagem = await _postagemRepo.ObterComRelacionamentosAsync(postagemId)
                ?? throw new DomainException("Postagem não encontrada");

            var usuarioExiste = await _usuarioRepo.ExisteIdAsync(dto.UsuarioId);
            if (!usuarioExiste)
                throw new DomainException("Usuário não encontrado");

            if (postagem.AutorId == dto.UsuarioId)
                throw new DomainException("Você não pode curtir sua própria postagem");

            // 4. Adicionar curtida
            postagem.AdicionarCurtida(postagemId,dto.UsuarioId);
            await _postagemRepo.SalvarAsync();
        }

        public async Task<List<PostagemResponseDto>> ListarPostagensPorUsuarioAsync(int usuarioId)
        {
            var postagens = await _postagemRepo.ListarPostagensPorUsuarioAsync(usuarioId);
            return postagens.Select(MapearParaDto).ToList();
        }

        private PostagemResponseDto MapearParaDto(Postagem postagem)
        {
            return new PostagemResponseDto
            {
                Id = postagem.Id,
                Autor = new UsuarioResumidoDto
                {
                    Id = postagem.Autor.Id,
                    Nome = postagem.Autor.Nome,
                    Email = postagem.Autor.Email
                },
                Conteudo = postagem.Conteudo,
                DataHora = postagem.DataHora,
                TotalCurtidas = postagem.Curtidas.Count,
                Comentarios = postagem.Comentarios.Select(c => new ComentarioResponseDto
                {
                    Id = c.Id,
                    Autor = new UsuarioResumidoDto
                    {
                        Id = c.Autor.Id,
                        Nome = c.Autor.Nome,
                        Email = c.Autor.Email
                    },
                    Texto = c.Texto,
                    DataHora = c.DataHora
                }).ToList()
            };
        }

        public async Task AdicionarComentarioAsync(int postagemId, ComentarioRequestDto dto)
        {
            var postagem = await _postagemRepo.ObterPorIdAsync(postagemId)
                ?? throw new DomainException("Postagem não encontrada");

            var usuario = await _usuarioRepo.ObterPorIdAsync(dto.UsuarioId)
                ?? throw new DomainException("Usuário não encontrado");

            postagem.AdicionarComentario(dto.UsuarioId, dto.Texto);
            await _postagemRepo.SalvarAsync();
        }
    }
}