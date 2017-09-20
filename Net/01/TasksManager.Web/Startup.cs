

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.Owin.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using TasksManager.Web.Castle;
using Utils;

namespace TasksManager.Web
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {


            var container = BootstrapContainer();

            appBuilder.Use((context, next) =>
            {
                context.Response.Headers.Remove("Server");
                return next.Invoke();
            });
            appBuilder.UseStageMarker(PipelineStage.PostAcquireState);

            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            ConfigureJsonSerializationSettings(config);
            //// Web API routes
            config.MapHttpAttributeRoutes();

            var httpDependencyResolver = new WindsorHttpDependencyResolver(container);
            config.DependencyResolver = httpDependencyResolver;

            appBuilder.UseWebApi(config);
        }

        private static void ConfigureJsonSerializationSettings(HttpConfiguration config)
        {
            var defaultSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>
                        {
                            new StringEnumConverter{ CamelCaseText = true },
                        }
            };
            JsonConvert.DefaultSettings = () => { return defaultSettings; };

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings = defaultSettings;
        }

        private IWindsorContainer BootstrapContainer()
        {
            var container = new WindsorContainer().Install(
               new ControllerInstaller(),
               new DefaultInstaller());
            var binPath = AssemblyPathExtension.AssemblyDir;
            var filter = new AssemblyFilter(binPath).
                FilterByAssembly(a => !a.IsDynamic);

            container.AddFacility<TypedFactoryFacility>();
            container.Kernel.Resolver.
                AddSubResolver(new CollectionResolver(container.Kernel));

            container.Install(
                FromAssembly.
                    InDirectory(filter));

            container.Register(
                Classes.
                    FromAssemblyInDirectory(filter).
                    BasedOn<ISingleton>().
                    WithServiceAllInterfaces().
                    LifestyleSingleton());

            return container;
        }
    }
}
