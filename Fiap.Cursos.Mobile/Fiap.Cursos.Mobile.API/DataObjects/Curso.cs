using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Fiap.Cursos.Mobile.API.DataObjects
{
    public class Curso : EntityData
    {
        public Curso() { }

        public Curso(string nome, string descricao, bool ativo = true)
        {
            Nome = nome;
            Descricao = descricao;
            Ativo = ativo;        
        }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        [JsonIgnore]
        public virtual Collection<CursoDisciplina> Disciplinas { get; set; } = new Collection<CursoDisciplina>();
    }
}