using Compentio.SourceMapper.Metadata;
using System.Text;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    internal class NinjectProcessorStrategy : IDependencyInjectionStrategy
    {
        public IResult GenerateCode(ISourcesMetadata sourcesMetadata)
        {
            var result = @$"// <mapper-source-generated />
                            // <generated-at '{System.DateTime.UtcNow}' />

            using System;
            using Ninject;

            { $"namespace Compentio.SourceMapper.DependencyInjection"}
            {{
               public static class {sourcesMetadata.DependencyInjection.DependencyInjectionClassName}
               {{
                   public static IKernel AddMappers(this IKernel kernel)
                   {{
                        { GenerateBuilder(sourcesMetadata) }
                        return kernel;
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
                builder.AppendLine($"kernel.Bind<{mapper.Namespace}.{mapper.Name}>().To<{mapper.Namespace}.{mapper.TargetClassName}>().InSingletonScope();");
            }

            return builder.ToString();
        }
    }
}