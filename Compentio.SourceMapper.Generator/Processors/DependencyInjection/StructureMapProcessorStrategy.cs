using Compentio.SourceMapper.Metadata;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    internal class StructureMapProcessorStrategy : AbstractDependencyInjectionStrategy
    {
        public override IResult GenerateCode(ISourcesMetadata sourcesMetadata)
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
                        { GenerateDependencyInjectionLines(sourcesMetadata) }
                        return builder;
                   }}
               }}
            }}
            ";

            return Result.Ok(result);
        }

        protected override string GenerateMappeLine(IMapperMetadata mapper)
        {
            return $"builder.For<{mapper.Namespace}.{mapper.Name}>().Singleton().Use<{mapper.Namespace}.{mapper.TargetClassName}>();";
        }
    }
}