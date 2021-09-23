using Compentio.SourceMapper.Diagnostics;
using Compentio.SourceMapper.Metadata;
using System.Collections.Generic;

namespace Compentio.SourceMapper.Processors
{
    interface IProcessorStrategy
    {
        /// <summary>
        /// Generates code for mappings
        /// </summary>
        /// <param name="mapperMetadata"></param>
        /// <returns>Generated code and diagnostics information. See also: <seealso cref="ProcessingResult"/></returns>
        IProcessingResult GenerateCode(IMapperMetadata mapperMetadata); 
    }

    internal abstract class AbstractProcessorStrategy : IProcessorStrategy
    {
        protected List<DiagnosticsInfo> _diagnostics = new();

        public abstract IProcessingResult GenerateCode(IMapperMetadata mapperMetadata);

        protected void PropertyMappingWarning(IPropertyMetadata metadata)
        {
            _diagnostics.Add(new DiagnosticsInfo
            {
                DiagnosticDescriptor = SourceMapperDescriptors.PropertyIsNotMapped,
                Metadata = metadata
            });
        }
    }
}
