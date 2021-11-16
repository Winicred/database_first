using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FirmaPersonal.Startup))]
namespace FirmaPersonal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
