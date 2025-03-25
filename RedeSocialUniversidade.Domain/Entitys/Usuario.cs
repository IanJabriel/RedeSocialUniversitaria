using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeSocialUniversidade.Domain.Entitys
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Curso { get; set; }

        public List<Usuario>Seguidores { get; set; }

        public Usuario() { 
            Seguidores = new List<Usuario>();
        }
    }
}
