using Compentio.SourceMapper.Diagnostics;
using Compentio.SourceMapper.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

namespace Compentio.SourceMapper.Processors
{
    interface IProcessorStrategy
    {
        /// <summary>
        /// Generates code for mappings
        /// </summary>
        /// <param name="mapperMetadata"></param>
        /// <returns>Mapper generated code</returns>
        string GenerateCode(IMapperMetadata mapperMetadata); 
        /// <summary>
        /// Returnes Warnings and Errors during code generation process. Should be used after <see cref="GenerateCode(IMapperMetadata)"/> method
        /// </summary>
        IEnumerable<DiagnosticsInfo> Diagnostics { get; }
    }

    internal abstract class AbstractProcessorStrategy : IProcessorStrategy
    {
        private readonly List<DiagnosticsInfo> _diagnostics = new();

        public IEnumerable<DiagnosticsInfo> Diagnostics => _diagnostics;

        public abstract string GenerateCode(IMapperMetadata mapperMetadata);
        protected string FormatCode(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot().NormalizeWhitespace();
            return root.ToFullString();
        }
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
