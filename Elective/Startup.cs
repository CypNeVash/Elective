using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Elective.Startup))]
namespace Elective
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
