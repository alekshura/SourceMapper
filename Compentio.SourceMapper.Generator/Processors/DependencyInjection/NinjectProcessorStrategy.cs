using Compentio.SourceMapper.Metadata;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    /// <summary>
    /// Implementation of Ninject dependency injection strategy
    /// </summary>
    internal class NinjectProcessorStrategy : AbstractDependencyInjectionStrategy
    {
        public override IResult GenerateCode(ISourcesMetadata sourcesMetadata)
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
                        { GenerateDependencyInjectionLines(sourcesMetadata) }
                        return kernel;
                   }}
               }}
            }}
            ";

            return Result.Ok(result);
        }

        protected override string GenerateMappeLine(IMapperMetadata mapper)
        {
            return $"kernel.Bind<{mapper.Namespace}.{mapper.Name}>().To<{mapper.Namespace}.{mapper.TargetClassName}>().InSingletonScope();";
        }
    }
}