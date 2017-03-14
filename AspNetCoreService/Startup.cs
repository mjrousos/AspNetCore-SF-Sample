using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac.Extensions.DependencyInjection;
using Autofac;

namespace AspNetCoreService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Autofac recommends against updating existing containers.
            // https://github.com/autofac/Autofac/issues/811
            // They say that a better practice is to treat Autofac containers as immutable
            // and create new ones, as needed. This would result in a different container
            // being used by the ASP.NET Core app and the hosting service, but the same
            // set of services would be available and may be a viable work around in
            // some cases.
            //
            // Note that it's also possible to register services with IWebHostBuilder.ConfigureServices,
            // which can be done in AspNetCoreService.cs outside of the ASP.NET Core's app's code.

#if USE_AUTOFAC_UPDATE
            // Setup Autofac container that combines existing container and ASP.NET Core services
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.Update(Program.Container);
            return new AutofacServiceProvider(Program.Container);
#else // USE_AUTOFAC_UPDATE
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MyAutofacModule());
            builder.Populate(services);
            return new AutofacServiceProvider(builder.Build());
#endif // USE_AUTOFAC_UPDATE
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
