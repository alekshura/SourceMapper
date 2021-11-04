using Autofac;
using Compentio.Example.WebAPI.Repository;
using Compentio.Example.WebAPI.Services;
using Compentio.SourceMapper.DependencyInjection;

namespace Compentio.Example.WebAPI
{
    /// <summary>
    /// Module with registration for Autofac
    /// </summary>
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BooksService>().As<IBooksService>().InstancePerDependency();
            builder.RegisterType<BooksRepository>().As<IBooksRepository>().SingleInstance();
            builder.AddMappers();
        }
    }
}