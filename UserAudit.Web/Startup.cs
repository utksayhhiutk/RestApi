using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UserAudit.Startup))]
namespace UserAudit
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
