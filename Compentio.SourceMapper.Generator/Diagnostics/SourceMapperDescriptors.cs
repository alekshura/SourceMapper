using Microsoft.CodeAnalysis;

namespace Compentio.SourceMapper.Diagnostics
{
    /// <summary>
    /// Source mapper diagnostics descriptors
    /// </summary>
    public static class SourceMapperDescriptors
    {
        private const string DiagnosticsDescriptorsUri = "https://github.com/alekshura/SourceMapper/wiki/Diagnostics#";

        public static readonly DiagnosticDescriptor ExpressionMustBeUsedInClass =
            new("SMAP0001", "Expression must be used in abstract class", "The Expression '{0}' must be in abstract class", "Design", DiagnosticSeverity.Warning, true,
                "Expression property can be used only in abstract classes. In interface it is ignored.", $"{DiagnosticsDescriptorsUri}smap0001");

        public static readonly DiagnosticDescriptor ConversionFunctionShouldBePublicOrProtected =
           new("SMAP0002", "Conversion function must be public or protected", "Conversion function '{0}' must be public or proteted", "Design", DiagnosticSeverity.Error, true,
               "Conversion function must be public or protected, because generated inherited mapping class uses it.", $"{DiagnosticsDescriptorsUri}smap0002");

        public static readonly DiagnosticDescriptor PropertyIsNotMapped =
           new("SMAP0003", "Property is not mapped", "The property '{0}' is not mapped", "Design", DiagnosticSeverity.Warning, true,
               "Source property for defined target property not found, thus property does not mapped.", $"{DiagnosticsDescriptorsUri}#smap0003");

        public static readonly DiagnosticDescriptor ExpectedInverseMethodName =
           new("SMAP0005", "Method name is expected", "Method name is required for mapping inversion mechanism, but not found any in source method: '{0}'", "Design", DiagnosticSeverity.Warning, true,
               "For method inversion mechanism, method name is required.", $"{DiagnosticsDescriptorsUri}#smap0005");

        public static readonly DiagnosticDescriptor MultipleInverseMethodName =
           new("SMAP0006", "Multiple inverse method name", "Inverse method name can be set only ones, but found multiple declarations in: '{0}'", "Design", DiagnosticSeverity.Warning, true,
               "Inverse method name can be set only ones for single method.", $"{DiagnosticsDescriptorsUri}#smap0006");

        public static readonly DiagnosticDescriptor UnexpectedError =
          new("SMAP0099", "Unexpected error", "Unexpected error ocured with message: '{0}'", "Design", DiagnosticSeverity.Error, true,
              "Unexpected exception occured during code generation.", $"{DiagnosticsDescriptorsUri}#smap0099");
    }    
}
