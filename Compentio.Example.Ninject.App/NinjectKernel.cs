using Compentio.Example.Ninject.App.Repository;
using Compentio.Example.Ninject.App.Services;
using Compentio.SourceMapper.DependencyInjection;
using Ninject;
using Ninject.Web.AspNetCore;
using Ninject.Web.AspNetCore.Hosting;

namespace Compentio.Example.Ninject.App
{
    /// <summary>
    /// Ninject Dependency Injection mechanism : kernel
    /// </summary>
    public class NinjectKernel
    {
        /// <summary>
        /// Method create kernel with all user defined dependency injections
        /// </summary>
        /// <returns></returns>
        public static IKernel CreateKernel()
        {
            var settings = new NinjectSettings();
            // Unfortunately, in .NET Core projects, referenced NuGet assemblies are not copied to the output directory
            // in a normal build which means that the automatic extension loading does not work _reliably_ and it is
            // much more reasonable to not rely on that and load everything explicitly.
            settings.LoadExtensions = false;

            var kernel = new AspNetCoreKernel(settings);

            kernel.Load(typeof(AspNetCoreHostConfiguration).Assembly);
            // User Dependency Injections
            kernel.Bind<IInvoiceService>().To<InvoiceService>().InSingletonScope();
            kernel.Bind<IInvoiceRepository>().To<InvoiceRepository>().InSingletonScope();
            // Using mappers from SourceMapper
            kernel.AddMappers();

            return kernel;
        }
    }
}