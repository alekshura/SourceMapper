using Compentio.SourceMapper.Metadata;

namespace Compentio.SourceMapper.DependencyInjection
{
    interface IDependencyInjectionStrategy
    {
        string GenerateCode(ISourcesMetadata sourcesMetadata);
    }
}
