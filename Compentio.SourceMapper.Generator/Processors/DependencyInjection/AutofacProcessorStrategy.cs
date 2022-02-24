using Compentio.SourceMapper.Metadata;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    internal class AutofacProcessorStrategy : AbstractDependencyInjectionStrategy
    {
        public override IResult GenerateCode(ISourcesMetadata sourcesMetadata)
        {
            var result = @$"// <mapper-source-generated />
                            // <generated-at '{System.DateTime.UtcNow}' />

            using System;
            using Autofac;
            using Autofac.Extensions.DependencyInjection;

            { $"namespace Compentio.SourceMapper.DependencyInjection"}
            {{
               public static class {sourcesMetadata.DependencyInjection.DependencyInjectionClassName}
               {{
                   public static ContainerBuilder AddMappers(this ContainerBuilder builder)
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
            return $"builder.RegisterType<{mapper.Namespace}.{mapper.TargetClassName}>().As<{mapper.Namespace}.{mapper.Name}>().SingleInstance();";
        }
    }
}