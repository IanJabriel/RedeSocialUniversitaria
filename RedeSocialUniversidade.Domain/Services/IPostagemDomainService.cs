using RedeSocialUniversidade.Domain.Entities;
using System;

namespace RedeSocialUniversidade.Domain.Services
{
    public interface IPostagemDomainService
    {
        void ValidarCriacaoPostagem(Usuario autor, string conteudo);
        void ValidarCurtida(Usuario usuario, Postagem postagem);
        void ValidarComentario(Usuario usuario, Postagem postagem, string textoComentario);
    }

    public class PostagemDomainService : IPostagemDomainService
    {
        public void ValidarCriacaoPostagem(Usuario autor, string conteudo)
        {
            if (autor == null)
                throw new Exception("Usuário inválido.");

            if (string.IsNullOrWhiteSpace(conteudo))
                throw new Exception("Conteúdo da postagem não pode ser vazio.");

            if (conteudo.Length > 1000)
                throw new Exception("Limite de 1000 caracteres excedido.");
        }

        public void ValidarCurtida(Usuario usuario, Postagem postagem)
        {
            if (usuario == null || postagem == null)
                throw new Exception("Dados inválidos para curtir.");

            if (postagem.Curtidas?.Any(c => c.UsuarioId == usuario.Id) == true)
                throw new Exception("Você já curtiu esta postagem.");
        }

        public void ValidarComentario(Usuario usuario, Postagem postagem, string textoComentario)
        {
            if (usuario == null || postagem == null)
                throw new Exception("Dados inválidos para comentar.");

            if (string.IsNullOrWhiteSpace(textoComentario))
                throw new Exception("Comentário não pode ser vazio.");

            if (textoComentario.Length > 300)
                throw new Exception("Comentário muito longo (máx. 300 caracteres).");
        }
    }
}