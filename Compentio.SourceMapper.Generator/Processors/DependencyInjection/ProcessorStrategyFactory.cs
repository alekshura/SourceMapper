using Compentio.SourceMapper.Metadata;
using System.Collections.Generic;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{    
    internal static class DependencyInjectionStrategyFactory
    {
        private readonly static Dictionary<DependencyInjectionType, IDependencyInjectionStrategy> _dependencyInjectionStrategies = new()
        {
            { DependencyInjectionType.DotNetCore, new DotnetCoreProcessorStrategy() }
        };

        internal static IDependencyInjectionStrategy? GetStrategy(ISourcesMetadata sourcesMetadata)
        {
            if (!_dependencyInjectionStrategies.ContainsKey(sourcesMetadata.DependencyInjection.DependencyInjectionType))
            {
                return null;
            }

            return _dependencyInjectionStrategies[sourcesMetadata.DependencyInjection.DependencyInjectionType];
        }
    }

    internal enum DependencyInjectionType
    {
        DotNetCore, Autofac, StructureMap, None
    }
}
