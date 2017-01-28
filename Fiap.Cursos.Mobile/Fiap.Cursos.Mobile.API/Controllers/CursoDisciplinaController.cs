using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Fiap.Cursos.Mobile.API.DataObjects;
using Fiap.Cursos.Mobile.API.Models;

namespace Fiap.Cursos.Mobile.API.Controllers
{
    [Authorize]
    public class CursoDisciplinaController : TableController<CursoDisciplina>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<CursoDisciplina>(context, Request);
        }

        // GET tables/CursoDisciplina/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<CursoDisciplina> GetCursoDisciplina(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/CursoDisciplina/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<CursoDisciplina> PatchCursoDisciplina(string id, Delta<CursoDisciplina> patch)
        {
            return UpdateAsync(id, patch);
        }
    }
}
