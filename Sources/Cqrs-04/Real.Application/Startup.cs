using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bus;
using Crud;
using Cruise;
using Cruise.Commands;
using InMemory.Crud;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NServiceBus;
using NServiceBus.Extensions.DependencyInjection;
using Unity;

namespace Real.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(IUnityContainer container)
        {
            // Could be used to register more types
            //container.RegisterType<IMessageHandler>(;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
            services.AddSingleton<IOptimisticRepository<CruiseEntity>>(new InMemoryOptimisticRepository<CruiseEntity>());
            services.AddSingleton<IRepository<RoomsForTripsEntity>>(new InMemoryRepository<RoomsForTripsEntity>());
            services.AddSingleton<IRepository<CruiseProjectionEntity>>(new InMemoryRepository<CruiseProjectionEntity>());
            services.AddMvc();
            var endpointConfiguration = new NServiceBus.EndpointConfiguration("Sample.Core");
            endpointConfiguration.UseTransport<LearningTransport>();

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var routerConfig = transport.Routing();
            routerConfig.RouteToEndpoint(
                assembly: typeof(CreateRoom).Assembly,
                destination: "Sample.Core");

            var type = typeof(IMessageHandler);
            foreach(var founded in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p)))
            {
                //services.AddSingleton(founded);
                //services.
                //Console.WriteLine(founded.FullName);
            }

            services.AddNServiceBus(endpointConfiguration);


            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
