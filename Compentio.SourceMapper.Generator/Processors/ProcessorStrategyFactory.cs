using Compentio.SourceMapper.Metadata;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Compentio.SourceMapper.Processors
{
    internal class ProcessorStrategyFactory
    {
        private readonly static Dictionary<TypeKind, IProcessorStrategy> _mappersStrategies = new()
        {
            { TypeKind.Interface, new InterfaceProcessorStrategy()},
            { TypeKind.Class, new ClassProcessorStrategy() }
        };

        internal static IProcessorStrategy GetStrategy(IMapperMetadata mapperMetadata)
        {
            if (!_mappersStrategies.ContainsKey(mapperMetadata.TypeKind))
            {
                return new InterfaceProcessorStrategy();
            }

            return _mappersStrategies[mapperMetadata.TypeKind];
        }
    }
}
