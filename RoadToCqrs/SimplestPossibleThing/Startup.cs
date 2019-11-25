using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Lib.Cqrs;
using Infrastructure.Lib.ServiceBus;
using Infrastructure.Mongo.Cqrs;
using Infrastructure.Mongo.Projections;
using Infrastructure.Rabbit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimplestPossibleThing.Lib;
using SimplestPossibleThing.Lib.Projection;
using Swashbuckle.AspNetCore.Swagger;

namespace SimplestPossibleThing
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var mongoDbConnectionString = "mongodb://localhost";
            var rabbitMQConnectionString = "localhost";

            var bus = new RabbitBus(rabbitMQConnectionString);
            services.AddSingleton<IBus>(bus);
            services.AddSingleton<IInventoryItemDetailsRepository>(new MongoInventoryItemDetailsRepository(mongoDbConnectionString));
            services.AddSingleton<IInventoryItemListRepository>(new MongoInventoryItemListRepository(mongoDbConnectionString));
            var sp = services.BuildServiceProvider();
            services.AddSingleton<IEntityStorage>(new MongoEntityStorage(sp.GetService<IBus>(), mongoDbConnectionString));
            sp = services.BuildServiceProvider();
            services.AddSingleton(new InventoryCommandHandlers(sp.GetService<IBus>(), sp.GetService<IEntityStorage>()));
            services.AddSingleton(new InventoryItemDetailView(sp.GetService<IBus>(), sp.GetService<IInventoryItemDetailsRepository>()));
            services.AddSingleton(new InventoryListView(sp.GetService<IBus>(), sp.GetService<IInventoryItemListRepository>()));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
