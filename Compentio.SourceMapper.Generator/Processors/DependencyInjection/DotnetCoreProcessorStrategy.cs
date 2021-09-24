using Compentio.SourceMapper.Metadata;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    internal class DotnetCoreProcessorStrategy : IDependencyInjectionStrategy
    {
        public IResult GenerateCode(ISourcesMetadata sourcesMetadata)
        {
            var result = @$"// <mapper-source-generated />
                            // <generated-at '{System.DateTime.UtcNow}' />

            using System;
            using Microsoft.Extensions.DependencyInjection;

            { $"namespace Compentio.SourceMapper.DependencyInjection"}
            {{
               public static class {sourcesMetadata.DependencyInjection.DependencyInjectionClassName}
               {{                  
                   public static IServiceCollection AddMappers(this IServiceCollection services)
                   {{
                        { GenerateServices(sourcesMetadata) }
                        return services;
                   }}                                  
               }}
            }}
            ";

            return Result.Ok(result);
        }

        private string GenerateServices(ISourcesMetadata sourcesMetadata)
        {
            var services = string.Empty;

            foreach (var mapper in sourcesMetadata.Mappers)
            {
                services += $"services.AddSingleton<{mapper.Namespace}.{mapper.Name}, {mapper.Namespace}.{mapper.TargetClassName}>();";
            }

            return services;
        }
    }
}
