using Microsoft.Owin;
using Owin;
[assembly: OwinStartupAttribute(typeof(PhotoSharer.Startup))]
namespace PhotoSharer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

    }

}