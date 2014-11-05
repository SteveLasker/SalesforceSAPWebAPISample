using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ContosoRepairManager.Startup))]
namespace ContosoRepairManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
