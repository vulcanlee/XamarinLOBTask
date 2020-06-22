using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(XamarinLOBTaskService.Startup))]

namespace XamarinLOBTaskService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}