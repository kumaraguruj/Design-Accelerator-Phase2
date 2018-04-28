using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DesignAccelerator.Startup))]
namespace DesignAccelerator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
