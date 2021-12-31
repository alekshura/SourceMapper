using Microsoft.AspNetCore.Hosting;
using Ninject.Web.AspNetCore.Hosting;
using Ninject.Web.Common.SelfHost;
using System;
using System.Linq;

namespace Compentio.Example.Ninject.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // The hosting model can be explicitly configured with the SERVER_HOSTING_MODEL environment variable.
            // See https://www.andrecarlucci.com/en/setting-environment-variables-for-asp-net-core-when-publishing-on-iis/ for
            // setting the variable in IIS.
            var model = Environment.GetEnvironmentVariable("SERVER_HOSTING_MODEL");
            // Command line arguments have higher precedence than environment variables
            model = args.FirstOrDefault(arg => arg.StartsWith("--use"))?.Substring(5) ?? model;

            var hostConfiguration = new AspNetCoreHostConfiguration(args)
                    .UseStartup<Startup>()
                    .UseWebHostBuilder(CreateWebHostBuilder)
                    .BlockOnStart();

            switch (model)
            {
                case "Kestrel":
                    hostConfiguration.UseKestrel();
                    break;

                case "HttpSys":
                    hostConfiguration.UseHttpSys();
                    break;

                case "IIS":
                case "IISExpress":
                    hostConfiguration.UseIIS();
                    break;

                default:
                    throw new ArgumentException($"Unknown hosting model '{model}'");
            }

            var host = new NinjectSelfHostBootstrapper(NinjectKernel.CreateKernel, hostConfiguration);
            host.Start();
        }

        public static IWebHostBuilder CreateWebHostBuilder()
        {
            return new DefaultWebHostConfiguration(null)
                .ConfigureAll()
                .GetBuilder();
        }
    }
}