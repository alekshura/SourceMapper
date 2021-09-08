using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Processors
{
    interface ISourceMetadata
    {
        string FileName { get; }
        string Namespace { get; }
        string MapperName { get; }
        string TargetClassName { get; }
        IEnumerable<MethodMetadata> MethodsMetadata { get; }
    }

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

        public IEnumerable<MethodMetadata> MethodsMetadata => _typeSymbol.GetMembers()
                    .Where(field => field.Kind == SymbolKind.Method)
                    .Select(method =>
                    {
                        return new MethodMetadata(method as IMethodSymbol);
                    });
    }
}
