using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using BullsAndCows.Data;


[assembly: OwinStartup(typeof(BullsAndCows.WebAPI.Startup))]

namespace BullsAndCows.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(GlobalConfiguration.Configuration);
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            RegisterMappings(kernel);
            return kernel;
        }

        private static void RegisterMappings(StandardKernel kernel)
        {
            kernel.Bind<IBullsAndCowsData>().To<BullsAndCowsData>().WithConstructorArgument("context", c => new BullsAndCowsDbContext());
        }
    }
}
