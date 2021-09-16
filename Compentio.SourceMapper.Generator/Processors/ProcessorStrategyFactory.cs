using Compentio.SourceMapper.Metadata;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Compentio.SourceMapper.Processors
{
    internal class ProcessorStrategyFactory
    {
        private readonly static Dictionary<TypeKind, IProcessorStrategy> _sourceStrategies = new()
        {
            { TypeKind.Interface, new InterfaceProcessorStrategy()},
            { TypeKind.Class, new ClassProcessorStrategy() }
        };

        internal static IProcessorStrategy GetStrategy(ISourceMetadata sourceMetadata)
        {
            if (!_sourceStrategies.ContainsKey(sourceMetadata.TypeKind))
            {
                return new InterfaceProcessorStrategy();
            }

            return _sourceStrategies[sourceMetadata.TypeKind];
        }
    }
}
