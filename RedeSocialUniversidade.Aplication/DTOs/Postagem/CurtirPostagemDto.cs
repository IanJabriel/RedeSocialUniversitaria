using System.ComponentModel.DataAnnotations;
public class CurtirPostagemDto
{
    [Required(ErrorMessage = "O ID do usuário é obrigatório")]
    public int UsuarioId { get; set; }
}