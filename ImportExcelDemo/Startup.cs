using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImportExcelDemo.Startup))]
namespace ImportExcelDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
