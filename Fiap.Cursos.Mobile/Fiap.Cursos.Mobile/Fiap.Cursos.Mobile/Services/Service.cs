using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.Cursos.Mobile.Services
{
    public class Service<T>
        where T : class
    {
        public async Task<IEnumerable<T>> GetAll()
        {
            var table = App.Client.GetTable<T>();
            return await table.ReadAsync();
        }

        public async Task<T> Insert(T entity)
        {
            var table = App.Client.GetTable<T>();
            await table.InsertAsync(entity);
            return entity;
        }

        protected MobileServiceClient Client
        {
            get { return App.Client; }
        }
    }
}
