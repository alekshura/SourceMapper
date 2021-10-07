using Compentio.SourceMapper.Metadata;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Compentio.SourceMapper.Processors
{
    /// <summary>
    /// Code processor factory that returns proper processod that depends on mapper definition type: abstract class or interface
    /// </summary>
    internal static class ProcessorStrategyFactory
    {
        private readonly static Dictionary<TypeKind, IProcessorStrategy> _mappersStrategies = new()
        {
            { TypeKind.Interface, new InterfaceProcessorStrategy()},
            { TypeKind.Class, new ClassProcessorStrategy() }
        };

        /// <summary>
        /// returns appropriate generator strategy based on mapper type kind <see cref="TypeKind"/>
        /// </summary>
        /// <param name="mapperMetadata"></param>
        /// <returns></returns>
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
