namespace RedeSocialUniversidade.Domain.Entities
{
    public class Evento
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Local { get; set; }

        public string Descricao  { get; set; }

        public DateTime DataHora { get; set; }
        public  bool ExigeInscricao { get; set; }
    }
}
