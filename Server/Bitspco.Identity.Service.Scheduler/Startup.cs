using Hangfire;
using Microsoft.Owin;
using Owin;
using Bitspco.Identity.Facade;
using System.Configuration;

[assembly: OwinStartup(typeof(Bitspco.Identity.Service.Scheduler.Startup))]
namespace Bitspco.Identity.Service.Scheduler
{
    public class Startup
    {
        private IdentityController controller;

        private IdentityController Controller
        {
            get
            {
                if (controller == null) controller = new IdentityController();
                return controller;
            }
        }
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);

            app.UseHangfireDashboard("", new DashboardOptions()
            {

            });
            app.UseHangfireServer();

        }
    }
}