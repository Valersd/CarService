using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarService.Web.Startup))]
namespace CarService.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
