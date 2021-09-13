using Microsoft.CodeAnalysis;
using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;

namespace Compentio.SourceMapper.Tests.CodeAnalysis
{
    internal class Symbol : ISymbol
    {
        public SymbolKind Kind { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }

        public string MetadataName { get; set; }

        public ISymbol ContainingSymbol { get; set; }

        public IAssemblySymbol ContainingAssembly { get; set; }

        public IModuleSymbol ContainingModule { get; set; }

        public INamedTypeSymbol ContainingType { get; set; }

        public INamespaceSymbol ContainingNamespace { get; set; }

        public bool IsDefinition { get; set; }

        public bool IsStatic { get; set; }

        public bool IsVirtual { get; set; }

        public bool IsOverride { get; set; }

        public bool IsAbstract { get; set; }

        public bool IsSealed { get; set; }

        public bool IsExtern { get; set; }

        public bool IsImplicitlyDeclared { get; set; }

        public bool CanBeReferencedByName { get; set; }

        public ImmutableArray<Location> Locations { get; set; }

        public ImmutableArray<SyntaxReference> DeclaringSyntaxReferences { get; set; }

        public Accessibility DeclaredAccessibility
        {
            get; set;
        }

        public ISymbol OriginalDefinition
        {
            get; set;
        }

        public bool HasUnsupportedMetadata
        {
            get; set;
        }

        public void Accept(SymbolVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public TResult? Accept<TResult>(SymbolVisitor<TResult> visitor)
        {
            throw new NotImplementedException();
        }

        public bool Equals([NotNullWhen(true)] ISymbol other, SymbolEqualityComparer equalityComparer)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ISymbol other)
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<AttributeData> GetAttributes()
        {
            throw new NotImplementedException();
        }

        public string GetDocumentationCommentId()
        {
            throw new NotImplementedException();
        }

        public string GetDocumentationCommentXml(CultureInfo preferredCulture = null, bool expandIncludes = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<SymbolDisplayPart> ToDisplayParts(SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }

        public string ToDisplayString(SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<SymbolDisplayPart> ToMinimalDisplayParts(SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }

        public string ToMinimalDisplayString(SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }
    }
}
