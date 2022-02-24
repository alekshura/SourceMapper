using Compentio.SourceMapper.Metadata;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    internal class DotnetCoreProcessorStrategy : AbstractDependencyInjectionStrategy
    {
        public override IResult GenerateCode(ISourcesMetadata sourcesMetadata)
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
                        { GenerateDependencyInjectionLines(sourcesMetadata) }
                        return services;
                   }}
               }}
            }}
            ";

            return Result.Ok(result);
        }

        protected override string GenerateMappeLine(IMapperMetadata mapper)
        {
            return $"services.AddSingleton<{mapper.Namespace}.{mapper.Name}, {mapper.Namespace}.{mapper.TargetClassName}>();";
        }
    }
}