using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Fiap.Cursos.Mobile.API.Startup))]

namespace Fiap.Cursos.Mobile.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}