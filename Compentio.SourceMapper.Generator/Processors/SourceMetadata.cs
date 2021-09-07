using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Processors
{
    internal class SourceMetadata : ISourceMetadata
    {
        private readonly ITypeSymbol _typeSymbol;

        internal SourceMetadata(ITypeSymbol typeSymbol)
        {
            _typeSymbol = typeSymbol;
        }
        public string FileName => $"{TargetClassName}.cs";
        public string MapperName => _typeSymbol.Name;
        public string Namespace => _typeSymbol.ContainingNamespace.ToString();
        
        public string TargetClassName
        {
            get
            {
                var className = _typeSymbol.Name.TrimStart('I', 'i');
                if (className.Equals(MapperName, StringComparison.InvariantCultureIgnoreCase))
                {
                    className = $"{MapperName}Impl";
                }
                return className;
            }
        }

        public IDictionary<string, IMethodSymbol> MethodsMap
        {
            get
            {
                var methodFormat = new SymbolDisplayFormat(parameterOptions: SymbolDisplayParameterOptions.IncludeName | SymbolDisplayParameterOptions.IncludeType,
                   memberOptions: SymbolDisplayMemberOptions.IncludeParameters,
                   typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

                return _typeSymbol.GetMembers()
                    .Where(field => field.Kind == SymbolKind.Method)
                    .Select(method =>
                    {
                        method.ToDisplayString(methodFormat);
                        var mapperInterfaceMethod = method as IMethodSymbol;
                        return new KeyValuePair<string, IMethodSymbol>($"{mapperInterfaceMethod?.ReturnType.ToDisplayString()} {method.ToDisplayString(methodFormat)}", mapperInterfaceMethod);
                    })
                    .ToDictionary(x => x.Key, x => x.Value);
            }
        }
    }
}
