using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(FinanceTracker.WebApi.AspApi.Startup))]

namespace FinanceTracker.WebApi.AspApi
{
    using System.Data.Entity;
    using System.Reflection;
    using System.Web.Http;
    using Data.DatabaseLink;
    using Data.DatabaseLink.Migrations;
    using Ninject.Web.Common.OwinHost;
    using Ninject.Web.WebApi.OwinHost;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<FinanceTrackerDbContext, Configuration>());

            AutoMapperConfig.RegisterMappings(Assembly.Load("FinanceTracker.WebApi.AspApi"));

            ConfigureAuth(app);

            var httpConfig = new HttpConfiguration();

            WebApiConfig.Register(httpConfig);

            httpConfig.EnsureInitialized();

            app
                .UseNinjectMiddleware(NinjectConfig.CreateKernel)
                .UseNinjectWebApi(httpConfig);
        }
    }
}
