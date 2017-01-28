using Fiap.Cursos.Mobile.API.DataObjects;
using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Fiap.Cursos.Mobile.API.Models
{
    public class ModuloDomainManager : EntityDomainManager<CursoDisciplina>
    {
        public ModuloDomainManager(DbContext context, HttpRequestMessage request) : base(context, request)
        {

        }

        public IQueryable<Modulo> QueryByCursoID(string cursoId)
        {
            var ctx = Context as MobileServiceContext;

            var items = (from cd in ctx.CursoDisciplinas
                         where cd.IdCurso == cursoId && cd.Ativo
                         group cd by cd.Modulo into g
                         select new
                         {
                             Titulo = "Módulo " + g.Key,
                             Disciplinas = g.Select(q => new { q.Id, q.Disciplina.Nome })
                         })
                        .AsEnumerable()
                        .Select(p => new Modulo
                        {
                            Titulo = p.Titulo,
                            Disciplinas = new List<KeyValuePair<string, string>>(p.Disciplinas.Select(q => new KeyValuePair<string, string>(q.Id, q.Nome)))
                        })
                        .AsQueryable();

            return items;
        }
    }
}