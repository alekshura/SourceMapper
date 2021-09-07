using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compentio.SourceMapper.Processors
{
    internal class InterfaceSourceMetadata : ISourceMetadata
    {
        private readonly ITypeSymbol _mapperInterface;

        internal InterfaceSourceMetadata(ITypeSymbol mapperInterface)
        {
            _mapperInterface = mapperInterface;
        }

        public string FileName => $"{TargetClassName}.cs";
        public string InterfaceName => _mapperInterface.Name;
        public string Namespace => _mapperInterface.ContainingNamespace.ToString();
        public string TargetClassName
        {
            get
            {
                var className = _mapperInterface.Name.TrimStart('I', 'i');
                if (className.Equals(InterfaceName, StringComparison.InvariantCultureIgnoreCase))
                {
                    className = $"{InterfaceName}Impl";
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

                return _mapperInterface.GetMembers()
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
