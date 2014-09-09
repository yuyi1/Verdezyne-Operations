using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Operations.Startup))]
namespace Operations
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
