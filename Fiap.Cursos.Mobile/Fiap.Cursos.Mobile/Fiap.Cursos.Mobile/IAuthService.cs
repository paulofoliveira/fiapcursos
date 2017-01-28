using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.Cursos.Mobile
{
    public interface IAuthService
    {
        Task<MobileServiceUser> LoginAsync();
    }
}
