using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FibrexSupplierPortal.Startup))]
namespace FibrexSupplierPortal
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
