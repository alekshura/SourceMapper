using Microsoft.CodeAnalysis;

namespace Compentio.SourceMapper.Diagnostics
{
    public static class SourceMapperDescriptors
    {
        public static readonly DiagnosticDescriptor EnumerationMustBePartial
         = new("SMAP0001",                               // id
               "Enumeration must be partial",           // title
               "The enumeration '{0}' must be partial", // message
               "SourceMapperAnalyzer",                          // category
               DiagnosticSeverity.Error,
               true);
    }
}
