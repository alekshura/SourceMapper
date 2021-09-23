using Microsoft.CodeAnalysis;

namespace Compentio.SourceMapper.Diagnostics
{
    /// <summary>
    /// Source mapper diagnostics descriptors
    /// </summary>
    internal static class SourceMapperDescriptors
    {
        public static readonly DiagnosticDescriptor ExpressionMustBeUsedInClass =
            new("SMAP0001", "Expression must be used in abstract class", "The Expression '{0}' must be in abstract class", "SourceMapperAnalyzer", DiagnosticSeverity.Warning, true);

        public static readonly DiagnosticDescriptor ConversionFunctionShouldBePublicOrProtected =
           new("SMAP0002", "Conversion function must be public or proteted", "Conversion function '{0}' must be public or proteted", "SourceMapperAnalyzer", DiagnosticSeverity.Error, true);

        public static readonly DiagnosticDescriptor PropertyIsNotMapped =
           new("SMAP0003", "Property is not mapped", "The property '{0}' is not mapped", "SourceMapperAnalyzer", DiagnosticSeverity.Warning, true);

        public static readonly DiagnosticDescriptor UnexpectedError =
          new("SMAP0099", "Unexpected error", "Unexpected error ocured with message: '{0}'", "SourceMapperAnalyzer", DiagnosticSeverity.Error, true);
    }
    
}
