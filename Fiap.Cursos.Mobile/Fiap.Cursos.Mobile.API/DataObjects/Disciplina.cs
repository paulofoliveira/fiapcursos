using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Fiap.Cursos.Mobile.API.DataObjects
{
    public class Disciplina : EntityData
    {
        public Disciplina() { }

        public Disciplina(string nome)
        {
            Nome = nome;
        }

        public string Nome { get; set; }

        [JsonIgnore]
        public virtual Collection<CursoDisciplina> Cursos { get; set; } = new Collection<CursoDisciplina>();
    }
}