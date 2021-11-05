using Autofac;
using Compentio.Example.Autofac.App.Repository;
using Compentio.Example.Autofac.App.Services;
using Compentio.SourceMapper.DependencyInjection;

namespace Compentio.Example.Autofac.App
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