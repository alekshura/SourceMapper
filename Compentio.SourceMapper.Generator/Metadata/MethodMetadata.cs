using Compentio.SourceMapper.Attributes;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Metadata
{
    interface IMethodMetadata
    {
        string MethodName { get; }
        ITypeMetadata ReturnType { get; }
        IEnumerable<ITypeMetadata> Parameters { get; }
        string MethodFullName { get; }
        IEnumerable<MappingAttribute> MappingAttributes { get; }
    }

    internal class MethodMetadata : IMethodMetadata
    {
        private readonly IMethodSymbol _methodSymbol;
        private readonly SymbolDisplayFormat _methodFullNameFormat = 
            new(parameterOptions: SymbolDisplayParameterOptions.IncludeName | SymbolDisplayParameterOptions.IncludeType,
                   memberOptions: SymbolDisplayMemberOptions.IncludeParameters,
                   typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);


        internal MethodMetadata(IMethodSymbol methodSymbol)
        {
            _methodSymbol = methodSymbol;
        }
        public string MethodName 
        {
            get
            {
                var parts = _methodSymbol.ToDisplayParts(SymbolDisplayFormat.CSharpShortErrorMessageFormat)
                    .Where(s => s.Kind == SymbolDisplayPartKind.MethodName);
                return $"{string.Join(string.Empty, parts)}";
            }
        }

        public ITypeMetadata ReturnType => new TypeMetadata(_methodSymbol.ReturnType);
        public IEnumerable<ITypeMetadata> Parameters => _methodSymbol.Parameters.Select(p => new ParameterTypeMetadata(p));

        public string MethodFullName => $"{ReturnType.FullName} {_methodSymbol?.ToDisplayString(_methodFullNameFormat)}";

        public IEnumerable<MappingAttribute> MappingAttributes => _methodSymbol.GetAttributes()
                    .Where(attribute => attribute is not null && attribute.AttributeClass?.Name == nameof(MappingAttribute))
                    .Select(attribute =>
                    {
                        var mappingAttr = new MappingAttribute();
                        mappingAttr.Source = attribute.NamedArguments.FirstOrDefault(x => x.Key == nameof(mappingAttr.Source)).Value.Value.ToString();
                        mappingAttr.Target = attribute.NamedArguments.FirstOrDefault(x => x.Key == nameof(mappingAttr.Target)).Value.Value.ToString();
                        return mappingAttr;
                    });
    }
}
