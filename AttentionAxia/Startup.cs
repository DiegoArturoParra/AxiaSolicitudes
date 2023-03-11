using Microsoft.Owin;
using Owin;
[assembly: OwinStartupAttribute(typeof(AttentionAxia.Startup))]
namespace AttentionAxia
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}