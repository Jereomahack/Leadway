using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LightWay.Startup))]
namespace LightWay
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
