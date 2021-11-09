using Compentio.Example.DotNetCore.App.Repositories;
using Compentio.Example.DotNetCore.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;
using Compentio.SourceMapper.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Compentio.ConsoleApp
{
    /// <summary>
    /// Sample code using standard DotNetCore Dependency Injection
    /// </summary>
    [ExcludeFromCodeCoverage]
    static class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            using IServiceScope serviceScope = host.Services.CreateScope();
            var notesService = serviceScope.ServiceProvider.GetRequiredService<INotesService>();
            var result = await notesService.GetNotes();
            var notes = result.ToList();
            Console.ReadKey();
            await host.RunAsync();        
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddTransient<INotesService, NotesService>()
                            .AddSingleton<INotesRepository, NotesRepository>()
                            .AddMappers());
    }
}
