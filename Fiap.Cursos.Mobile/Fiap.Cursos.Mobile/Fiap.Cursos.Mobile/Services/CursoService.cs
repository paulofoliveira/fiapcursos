using Fiap.Cursos.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.Cursos.Mobile.Services
{
    public class CursoService : Service<Curso>
    {
        public async Task<IEnumerable<Curso>> GetByFiltro(string filter)
        {
            var table = App.Client.GetTable<Curso>();
            var query = table.Where(p => p.Nome.Contains(filter));
            return await table.ReadAsync(query);
        }
    }
}
