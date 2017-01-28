using Fiap.Cursos.Mobile.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fiap.Cursos.Mobile.Services
{
    public class DisciplinaService : Service<Disciplina>
    {
        public async Task<IEnumerable<DisciplinaGrupo>> GetAllDisciplinasByCursoIdAsync(string cursoId)
        {
            var arguments = new Dictionary<string, string>() { { "cursoId", cursoId } };
            IEnumerable<Modulo> items = await Client.InvokeApiAsync<IEnumerable<Modulo>>("Modulo", HttpMethod.Get, arguments);
            return items.Select(p => new DisciplinaGrupo(p.Disciplinas) { Modulo = p.Titulo });
        }

        public async Task<Disciplina> GetByIDAsync(string id)
        {
            var table = Client.GetTable<Disciplina>();
            return await table.LookupAsync(id);
        }
    }

}
