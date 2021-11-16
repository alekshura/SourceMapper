using Compentio.SourceMapper.Metadata;
using System;
using System.Text;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    internal class StructureMapProcessorStrategy : IDependencyInjectionStrategy
    {
        public IResult GenerateCode(ISourcesMetadata sourcesMetadata)
        {
            var result = @$"// <mapper-source-generated />
                            // <generated-at '{System.DateTime.UtcNow}' />

            using System;
            using StructureMap;

            { $"namespace Compentio.SourceMapper.DependencyInjection"}
            {{
               public static class {sourcesMetadata.DependencyInjection.DependencyInjectionClassName}
               {{                  
                   public static ConfigurationExpression AddMappers(this ConfigurationExpression builder)
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
            if (sourcesMetadata.Mappers == null) return string.Empty;

            var builder = new StringBuilder();

            foreach (var mapper in sourcesMetadata.Mappers)
            {
                builder.AppendLine($"builder.For<{mapper.Namespace}.{mapper.Name}>().Singleton().Use<{mapper.Namespace}.{mapper.TargetClassName}>();");
            }

            return builder.ToString();
        }
    }
}
