using Compentio.SourceMapper.Metadata;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    interface IDependencyInjectionStrategy
    {
        string GenerateCode(ISourcesMetadata sourcesMetadata);
    }
}
