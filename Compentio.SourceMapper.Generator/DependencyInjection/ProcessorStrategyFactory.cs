using Compentio.SourceMapper.Metadata;
using System.Collections.Generic;

namespace Compentio.SourceMapper.DependencyInjection
{    
    internal class DependencyInjectionStrategyFactory
    {
        private readonly static Dictionary<DependencyInjectionType, IDependencyInjectionStrategy> _dependencyInjectionStrategies = new()
        {
            { DependencyInjectionType.DotNetCore, new DotnetCoreProcessorStrategy() }
        };

        internal static IDependencyInjectionStrategy GetStrategy(ISourcesMetadata sourcesMetadata)
        {
            if (!_dependencyInjectionStrategies.ContainsKey(sourcesMetadata.DependencyInjection.DependencyInjectionType))
            {
                return new DotnetCoreProcessorStrategy();
            }

            return _dependencyInjectionStrategies[sourcesMetadata.DependencyInjection.DependencyInjectionType];
        }
    }

    internal enum DependencyInjectionType
    {
        DotNetCore, Castle, Autofac, StructureMap, Ninject, None
    }
}
