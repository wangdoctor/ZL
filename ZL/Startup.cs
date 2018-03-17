using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ZL.Startup))]
namespace ZL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
