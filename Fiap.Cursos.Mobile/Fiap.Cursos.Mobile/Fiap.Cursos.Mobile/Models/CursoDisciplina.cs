using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.Cursos.Mobile.Models
{
    public class CursoDisciplina
    {
        public string Id { get; set; }
        public byte Modulo { get; set; }
        public bool Ativo { get; set; }
        public string IdDisciplina { get; set; }
        public string IdCurso { get; set; }
    }
}
