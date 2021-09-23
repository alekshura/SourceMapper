using Compentio.SourceMapper.Metadata;
using Microsoft.CodeAnalysis;

namespace Compentio.SourceMapper.Diagnostics
{
    internal class DiagnosticsInfo
    {
        internal DiagnosticDescriptor? DiagnosticDescriptor { get; set; }
        internal IMetadata? Metadata { get; set; }
    }
}
