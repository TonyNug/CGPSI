using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CGPSI.AbsenceManagement.Startup))]
namespace CGPSI.AbsenceManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
