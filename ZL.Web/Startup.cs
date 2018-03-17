using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ZL.Web.Startup))]
namespace ZL.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
