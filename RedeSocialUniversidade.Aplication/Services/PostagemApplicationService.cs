using RedeSocialUniversidade.Application.DTOs.Postagem;
using RedeSocialUniversidade.Application.DTOs.Shared;
using RedeSocialUniversidade.Domain.Entities;
using RedeSocialUniversidade.Domain.Exceptions;
using RedeSocialUniversidade.Domain.Interface;
using RedeSocialUniversidade.Domain.Services;

namespace RedeSocialUniversidade.Application.Services
{
    public class PostagemAppService
    {
        private readonly IPostagemRepository _postagemRepo;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IPostagemDomainService _postagemDomainService;

        public PostagemAppService(
            IPostagemRepository postagemRepo,
            IUsuarioRepository usuarioRepo,
            IPostagemDomainService postagemDomainService)
        {
            _postagemRepo = postagemRepo;
            _usuarioRepo = usuarioRepo;
            _postagemDomainService = postagemDomainService;
        }

        public async Task<PostagemResponseDto> CriarPostagemAsync(CriarPostagemDto dto)
        {
            var usuario = await _usuarioRepo.ObterPorIdAsync(dto.AutorId)
                ?? throw new DomainException("Usuário não encontrado");

            _postagemDomainService.ValidarCriacaoPostagem(usuario, dto.Conteudo);

            var postagem = new Postagem(dto.AutorId, dto.Conteudo);
            await _postagemRepo.AdicionarAsync(postagem);
            await _postagemRepo.SalvarAsync();

            return MapearParaDto(await _postagemRepo.ObterComRelacionamentosAsync(postagem.Id));
        }

        public async Task<PostagemResponseDto> ObterPostagemAsync(int id)
        {
            var postagem = await _postagemRepo.ObterComRelacionamentosAsync(id)
                ?? throw new DomainException("Postagem não encontrada");

            return MapearParaDto(postagem);
        }

        public async Task CurtirPostagemAsync(int postagemId, CurtirPostagemDto dto)
        {
            var postagem = await _postagemRepo.ObterComRelacionamentosParaEdicaoAsync(postagemId)
                ?? throw new DomainException("Postagem não encontrada");

            var usuario = await _usuarioRepo.ObterPorIdAsync(dto.UsuarioId)
                ?? throw new DomainException("Usuário não encontrado");

            _postagemDomainService.ValidarCurtida(usuario, postagem);

            postagem.AdicionarCurtida(postagemId, dto.UsuarioId);
            await _postagemRepo.SalvarAsync();
        }

        public async Task<List<PostagemResponseDto>> ListarPostagensPorUsuarioAsync(int usuarioId)
        {
            var postagens = await _postagemRepo.ListarPostagensPorUsuarioAsync(usuarioId);
            return postagens.Select(MapearParaDto).ToList();
        }

        public async Task AdicionarComentarioAsync(int postagemId, ComentarioRequestDto dto)
        {
            var postagem = await _postagemRepo.ObterComRelacionamentosParaEdicaoAsync(postagemId)
                ?? throw new DomainException("Postagem não encontrada");

            var usuario = await _usuarioRepo.ObterPorIdAsync(dto.UsuarioId)
                ?? throw new DomainException("Usuário não encontrado");

            _postagemDomainService.ValidarComentario(usuario, postagem, dto.Texto);

            postagem.AdicionarComentario(dto.UsuarioId, dto.Texto);

            await _postagemRepo.SalvarAsync();
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
    }
}