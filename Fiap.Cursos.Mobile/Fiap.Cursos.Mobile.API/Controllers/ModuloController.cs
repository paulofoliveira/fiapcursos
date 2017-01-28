using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Linq;
using Fiap.Cursos.Mobile.API.Models;

namespace Fiap.Cursos.Mobile.API.Controllers
{
    [AllowAnonymous]
    [MobileAppController]
    public class ModuloController : ApiController
    {   
        public IQueryable<Modulo> GetDisciplinasByCursoID(string cursoId)
        {
            var dm = new ModuloDomainManager(new MobileServiceContext(), Request);
            return dm.QueryByCursoID(cursoId);
        }
    }
}
