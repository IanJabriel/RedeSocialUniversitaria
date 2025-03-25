using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeSocialUniversidade.Domain.Entitys
{
    public class Evento
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Local { get; set; }

        public string Descrição  { get; set; }

        public DateTime DataHora { get; set; }
    }
}
