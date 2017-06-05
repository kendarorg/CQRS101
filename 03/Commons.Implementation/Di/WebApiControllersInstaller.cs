using System.Web.Http.Controllers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Commons.Implementation.Castle
{
    public class WebApiControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var filter = new AssemblyFilter(CastleInitializer.BinPath).
                FilterByAssembly(a => !a.IsDynamic);
            
            

            container.Register(
                Classes.
                    FromAssemblyInDirectory(filter).
                    BasedOn<IHttpController>(). //Web API
                    If(c => c.Name.EndsWith("Controller")).
                    LifestyleTransient()
                );
        }
    }
}
