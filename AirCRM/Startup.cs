using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TravelCRM.Startup))]
namespace TravelCRM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
