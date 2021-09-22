using Microsoft.CodeAnalysis;

namespace Compentio.SourceMapper.Diagnostics
{
    /// <summary>
    /// Source mapper diagnostics descriptors
    /// </summary>
    public static class SourceMapperDescriptors
    {
        public static readonly DiagnosticDescriptor ExpressionMustBeUsedInClass = 
            new("SMAP0001", "Expression must be used in abstract class", "The Expression '{0}' must be in abstract class",  "SourceMapperAnalyzer", DiagnosticSeverity.Warning, true);
        
        public static readonly DiagnosticDescriptor ConversionFunctionShouldBePublicOrProtected =
           new("SMAP0002", "Conversion function must be public or proteted", "Conversion function {0} must be public or proteted", "SourceMapperAnalyzer", DiagnosticSeverity.Error, true);
    }
}
