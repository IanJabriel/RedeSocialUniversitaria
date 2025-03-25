using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeSocialUniversidade.Domain.Entitys
{
    public class Postagem
    {
        public int Id { get; set; }

        public string Autor { get; set; }

        public string Conteudo { get; set; }

        public DateTime DataHora { get; set; }
    }
}
