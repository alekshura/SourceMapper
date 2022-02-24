using Compentio.SourceMapper.Metadata;
using System.Text;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    /// <summary>
    /// Interface for dependency injection strategy
    /// </summary>
    internal interface IDependencyInjectionStrategy
    {
        IResult GenerateCode(ISourcesMetadata sourcesMetadata);
    }

    /// <summary>
    /// Base abstract for dependency injection strategy
    /// </summary>
    internal abstract class AbstractDependencyInjectionStrategy : IDependencyInjectionStrategy
    {
        public abstract IResult GenerateCode(ISourcesMetadata sourcesMetadata);

        /// <summary>
        /// Generate all mappers definitions from metadata <see cref="SourcesMetadata" />
        /// </summary>
        /// <param name="sourcesMetadata"></param>
        /// <returns></returns>
        protected string GenerateDependencyInjectionLines(ISourcesMetadata sourcesMetadata)
        {
            if (sourcesMetadata.Mappers == null) return string.Empty;

            var kernel = new StringBuilder();

            foreach (var mapper in sourcesMetadata.Mappers)
            {
                kernel.AppendLine(GenerateMappeLine(mapper));
            }

            return kernel.ToString();
        }

        /// <summary>
        /// Generate single mapper line definition <see cref="MapperMetadata" />
        /// </summary>
        /// <param name="mapper"></param>
        /// <returns></returns>
        protected abstract string GenerateMappeLine(IMapperMetadata mapper);
    }
}
