using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fiap.Cursos.Mobile.API.Models
{
    public class Modulo
    {
        public string Titulo { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Disciplinas { get; set; } = new HashSet<KeyValuePair<string, string>>();
    }
}