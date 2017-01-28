using Fiap.Cursos.Mobile.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.Cursos.Mobile.Services
{
    public class CursoDisciplinaService
    {
        private MobileServiceClient Client
        {
            get { return App.Client; }
        }

        public async Task<CursoDisciplina> GetByIDAsync(string id)
        {
            var table = Client.GetTable<CursoDisciplina>();
            return await table.LookupAsync(id);
        }

        public async Task UpdateAsync(CursoDisciplina item)
        {
            var table = Client.GetTable<CursoDisciplina>();
            await table.UpdateAsync(item);
        }
    }
}
