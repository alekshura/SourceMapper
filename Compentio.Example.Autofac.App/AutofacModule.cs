using Autofac;
using Compentio.Example.Autofac.App.Mapper;
using Compentio.Example.Autofac.App.Repository;
using Compentio.Example.Autofac.App.Services;
using Compentio.SourceMapper.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Compentio.Example.Autofac.App
{
    /// <summary>
    /// Module with registration for Autofac
    /// </summary>
    [ExcludeFromCodeCoverage]
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