using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Fiap.Cursos.Mobile.Droid;
using Xamarin.Forms;
using Android.Content;

[assembly: Dependency(typeof(AndroidAuthService))]

namespace Fiap.Cursos.Mobile.Droid
{
    public class AndroidAuthService : IAuthService
    {
        public async Task<MobileServiceUser> LoginAsync()
        {
            MobileServiceClient client = App.Client;
            return await client.LoginAsync(Forms.Context as Context, MobileServiceAuthenticationProvider.Google);
        }
    }
}