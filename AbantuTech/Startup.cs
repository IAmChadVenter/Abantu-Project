using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AbantuTech.Startup))]
namespace AbantuTech
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
