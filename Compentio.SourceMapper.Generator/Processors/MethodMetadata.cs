using Compentio.SourceMapper.Attributes;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Processors
{
    interface IMethodMetadata
    {
        string MethodName { get; }
        ITypeSymbol ReturnType { get; }
        IEnumerable<ITypeSymbol> Parameters { get; }
        string MethodFullName { get; }
        IEnumerable<MappingAttribute> MappingAttributes { get; }
    }

    internal class MethodMetadata : IMethodMetadata
    {
        private readonly IMethodSymbol _methodSymbol;
        private readonly SymbolDisplayFormat _methodFormat = 
            new(parameterOptions: SymbolDisplayParameterOptions.IncludeName | SymbolDisplayParameterOptions.IncludeType,
                   memberOptions: SymbolDisplayMemberOptions.IncludeParameters,
                   typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

        internal MethodMetadata(IMethodSymbol methodSymbol)
        {
            _methodSymbol = methodSymbol;
        }
        public string MethodName => _methodSymbol.ToDisplayString();
        public ITypeSymbol ReturnType => _methodSymbol.ReturnType;
        public IEnumerable<ITypeSymbol> Parameters => _methodSymbol.Parameters.Select(p => p.Type);

        public string MethodFullName => $"{_methodSymbol?.ReturnType.ToDisplayString()} {_methodSymbol?.ToDisplayString(_methodFormat)}";

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
