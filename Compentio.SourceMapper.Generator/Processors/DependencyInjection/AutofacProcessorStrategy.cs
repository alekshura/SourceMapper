using Compentio.SourceMapper.Metadata;
using System;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    internal class AutofacProcessorStrategy : IDependencyInjectionStrategy
    {
        public IResult GenerateCode(ISourcesMetadata sourcesMetadata)
        {
            var result = @$"// <mapper-source-generated />
                            // <generated-at '{System.DateTime.UtcNow}' />

            using System;
            using Microsoft.Extensions.DependencyInjection;
            using Autofac;
            using Autofac.Extensions.DependencyInjection;

            { $"namespace Compentio.SourceMapper.DependencyInjection"}
            {{
               public static class {sourcesMetadata.DependencyInjection.DependencyInjectionClassName}
               {{                  
                   public static ContainerBuilder AddMappers(this ContainerBuilder builder)
                   {{
                        { GenerateBuilder(sourcesMetadata) }
                        return builder;
                   }}                                  
               }}
            }}
            ";

            return Result.Ok(result);
        }

        private string GenerateBuilder(ISourcesMetadata sourcesMetadata)
        {
            var builder = string.Empty;

            foreach (var mapper in sourcesMetadata.Mappers)
            {
                builder += $"builder.RegisterType<{mapper.Namespace}.{mapper.TargetClassName}>().As<{mapper.Namespace}.{mapper.Name}>().SingleInstance();";
            }

            return builder;
        }
    }
}
