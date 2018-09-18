using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IAproject.Startup))]
namespace IAproject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
