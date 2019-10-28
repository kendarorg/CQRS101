using Crud;
using Cruise;
using Cruise.Commands;
using InMemory.Crud;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Extensions.DependencyInjection;

namespace Real.Application
{
    public class Startup
    {
        const string SAMPLE_ENDPOINT = "CQRS101";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
            services.AddSingleton<IOptimisticRepository<CruiseEntity>>(new InMemoryOptimisticRepository<CruiseEntity>());
            services.AddSingleton<IRepository<RoomsForTripsEntity>>(new InMemoryRepository<RoomsForTripsEntity>());
            services.AddSingleton<IRepository<CruiseProjectionEntity>>(new InMemoryRepository<CruiseProjectionEntity>());
            services.AddMvc();
            var endpointConfiguration = new NServiceBus.EndpointConfiguration(SAMPLE_ENDPOINT);
            endpointConfiguration.UseTransport<LearningTransport>();

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var routerConfig = transport.Routing();
            routerConfig.RouteToEndpoint(
                assembly: typeof(CreateRoom).Assembly,
                destination: SAMPLE_ENDPOINT);

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
