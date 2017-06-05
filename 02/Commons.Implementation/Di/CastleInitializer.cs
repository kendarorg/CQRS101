using System;
using System.IO;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Castle.Core.Logging;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Commons.Implementation.Castle
{
    public static class CastleInitializer
    {
        private static IWindsorContainer _container;

        public static IWindsorContainer Container
        {
            get
            {
                return _container;
            }
        }

        public static string GetAssemblyDir(this Assembly asm)
        {
            var codeBaseAbsolutePathUri = new Uri(asm.GetName().CodeBase).AbsolutePath;
            codeBaseAbsolutePathUri = codeBaseAbsolutePathUri.Replace("%20", " ");
            return Path.GetDirectoryName(codeBaseAbsolutePathUri);
        }

        public static string BinPath { get; private set; }

        public static void Initialize()
        {
            BinPath = Assembly.GetExecutingAssembly().GetAssemblyDir();

            _container = new WindsorContainer();


            ((IKernelInternal)_container.Kernel).Logger = new ConsoleLogger();


            _container.AddFacility<TypedFactoryFacility>();
            _container.Kernel.Resolver.
                AddSubResolver(new CollectionResolver(_container.Kernel));
            //_container.Kernel.Resolver.
            //    AddSubResolver(new DefaultDependencyResolver());

            var filter = new AssemblyFilter(BinPath).
                FilterByAssembly(a => !a.IsDynamic);

            _container.Install(
                FromAssembly.
                    InDirectory(filter));

            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new WindsorWebApiControllerActivator(_container));

            _container.Register(
                Classes.
                    FromAssemblyInDirectory(filter).
                    BasedOn<IService>().
                    WithServiceAllInterfaces().
                    LifestyleSingleton(),
                Classes.
                    FromAssemblyInDirectory(filter).
                    BasedOn<IComponent>().
                    WithServiceAllInterfaces().
                    LifestyleTransient()
             );


        }
    }
}
