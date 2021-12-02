using Compentio.SourceMapper.Metadata;
using System.Text;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    internal class AutofacProcessorStrategy : IDependencyInjectionStrategy
    {
        public IResult GenerateCode(ISourcesMetadata sourcesMetadata)
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
                builder.AppendLine($"builder.RegisterType<{mapper.Namespace}.{mapper.TargetClassName}>().As<{mapper.Namespace}.{mapper.Name}>().SingleInstance();");
            }

            return builder.ToString();
        }
    }
}