using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fiap.Cursos.Mobile.API.DataObjects
{
    public class CursoDisciplina : EntityData
    {
        public CursoDisciplina() { }
        public CursoDisciplina(Curso curso, Disciplina disciplina, byte modulo, bool ativo = true)
        {
            Curso = curso;
            Disciplina = disciplina;
            Modulo = modulo;
            Ativo = ativo;
        }

        public byte Modulo { get; set; }
        public bool Ativo { get; set; }

        public string IdDisciplina { get; set; }

        public string IdCurso { get; set; }

        [JsonIgnore]
        [ForeignKey("IdDisciplina")]
        public virtual Disciplina Disciplina { get; set; }

        [JsonIgnore]
        [ForeignKey("IdCurso")]
        public virtual Curso Curso { get; set; }
    }
}