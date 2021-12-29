using Compentio.SourceMapper.Metadata;
using System.Collections.Generic;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{    
    internal static class DependencyInjectionStrategyFactory
    {
        internal readonly static Dictionary<DependencyInjectionType, IDependencyInjectionStrategy> DependencyInjectionStrategies = new()
        {
            { DependencyInjectionType.DotNetCore, new DotnetCoreProcessorStrategy() },
            { DependencyInjectionType.Autofac, new AutofacProcessorStrategy() },
            { DependencyInjectionType.StructureMap, new StructureMapProcessorStrategy() },
            { DependencyInjectionType.Ninject, new NinjectProcessorStrategy() }
        };

        internal static IDependencyInjectionStrategy? GetStrategy(ISourcesMetadata sourcesMetadata)
        {
            if (!DependencyInjectionStrategies.ContainsKey(sourcesMetadata.DependencyInjection.DependencyInjectionType))
            {
                return null;
            }

            return DependencyInjectionStrategies[sourcesMetadata.DependencyInjection.DependencyInjectionType];
        }
    }

    internal enum DependencyInjectionType
    {
        DotNetCore, Autofac, StructureMap, Ninject, None
    }
}
