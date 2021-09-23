using Compentio.SourceMapper.Metadata;
using Microsoft.CodeAnalysis;
using System;

namespace Compentio.SourceMapper.Diagnostics
{
    internal class DiagnosticsInfo
    {
        internal DiagnosticDescriptor? DiagnosticDescriptor { get; set; }
        internal IMetadata? Metadata { get; set; }
        internal Exception? Exception { get; set; }
        internal string Message => Exception is not null ? 
            $"Message: {Exception.Message}, StackTrace: {Exception.StackTrace}" : Metadata.Name;
    }
}
