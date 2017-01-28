using System.Linq;
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
    public class CursoController : TableController<Curso>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Curso>(context, Request);
        }

        [AllowAnonymous]
        // GET tables/Curso
        public IQueryable<Curso> GetAllCurso()
        {
            return Query(); 
        }

        // GET tables/Curso/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Curso> GetCurso(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Curso/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Curso> PatchCurso(string id, Delta<Curso> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Curso
        public async Task<IHttpActionResult> PostCurso(Curso item)
        {
            Curso current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Curso/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteCurso(string id)
        {
             return DeleteAsync(id);
        }
    }
}
