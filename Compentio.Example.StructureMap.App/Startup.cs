using Compentio.Example.StructureMap.App.Repository;
using Compentio.Example.StructureMap.App.Services;
using Compentio.SourceMapper.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StructureMap;
using System.Diagnostics.CodeAnalysis;

namespace Compentio.Example.StructureMap.App
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        /// <summary>
        /// Registration directly with StructureMap.
        /// </summary>
        public void ConfigureContainer(Container builder)
        {
            builder.Configure(config =>
            {
                config.For<IInvoiceService>().Transient().Use<InvoiceService>();
                config.For<IInvoiceRepository>().Singleton().Use<InvoiceRepository>();
                config.AddMappers();
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}