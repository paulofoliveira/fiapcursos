using System;
using System.Collections.Generic;

namespace Fiap.Cursos.Mobile.Models
{
    public class DisciplinaGrupo : List<KeyValuePair<string, string>>
    {
        public DisciplinaGrupo(IEnumerable<KeyValuePair<string, string>> disciplinas)
        {
            foreach (var disciplina in disciplinas) Add(disciplina);
        }

        public string Modulo { get; set; }
    }
}