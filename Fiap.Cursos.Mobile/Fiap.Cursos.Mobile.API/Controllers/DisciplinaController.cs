using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Fiap.Cursos.Mobile.API.DataObjects;
using Fiap.Cursos.Mobile.API.Models;
using System;

namespace Fiap.Cursos.Mobile.API.Controllers
{
    [Authorize]
    public class DisciplinaController : TableController<Disciplina>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Disciplina>(context, Request);
        }

        [AllowAnonymous]
        // GET tables/Disciplina
        public IQueryable<Disciplina> GetAllDisciplina()
        {
            return Query();
        }

        // GET tables/Disciplina/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Disciplina> GetDisciplina(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Disciplina/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Disciplina> PatchDisciplina(string id, Delta<Disciplina> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Disciplina
        public async Task<IHttpActionResult> PostDisciplina(Disciplina item)
        {
            Disciplina current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Disciplina/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteDisciplina(string id)
        {
             return DeleteAsync(id);
        }
    }
}
