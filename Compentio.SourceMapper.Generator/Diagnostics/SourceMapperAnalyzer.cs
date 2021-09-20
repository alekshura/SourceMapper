using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Compentio.SourceMapper.Diagnostics
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SourceMapperAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(SourceMapperDescriptors.EnumerationMustBePartial);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            // TODO
           
            context.RegisterSymbolAction(AnalyzeNamedType, SymbolKind.NamedType);
        }
        private static void AnalyzeNamedType(SymbolAnalysisContext context)
        {
            //var type = context.Symbol as INamedTypeSymbol;

            //if (type?.TypeKind == TypeKind.Class)
            //{
            //    var error = Diagnostic.Create(SourceMapperDescriptors.EnumerationMustBePartial,
            //                           type?.Locations.FirstOrDefault(),
            //                           type?.Name);
            //    context.ReportDiagnostic(error);
            //}
        }
    }
}
