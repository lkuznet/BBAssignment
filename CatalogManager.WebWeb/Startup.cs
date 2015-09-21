using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CatalogManager.WebWeb.Startup))]
namespace CatalogManager.WebWeb
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
