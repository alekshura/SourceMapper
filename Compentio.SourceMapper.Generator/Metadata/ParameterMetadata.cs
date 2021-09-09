using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Metadata
{
    interface IParameterMetadata
    {
        string Name { get; }
        string FullName { get; }
        IEnumerable<IPropertySymbol?> Properties { get; }
    }

    internal class ParameterMetadata : IParameterMetadata
    {
        private readonly IParameterSymbol _parameterSymbol;

        internal ParameterMetadata(IParameterSymbol parameterSymbol)
        {
            _parameterSymbol = parameterSymbol;
        }

        public string Name => _parameterSymbol.Name;
        public string FullName => _parameterSymbol.ToDisplayString();

        public IEnumerable<IPropertySymbol> Properties => _parameterSymbol.Type.GetMembers()
            .Select(member => member as IPropertySymbol)
            .Where(member => member is not null);
    }

    internal class ReturnParameterMetadata : IParameterMetadata
    {
        private readonly ITypeSymbol _typeSymbol;

        internal ReturnParameterMetadata(ITypeSymbol typeSymbol)
        {
            _typeSymbol = typeSymbol;
        }

        public string Name => _typeSymbol.Name;
        public string FullName => _typeSymbol.ToDisplayString();

        public ITypeSymbol Type => _typeSymbol;

        public IEnumerable<IPropertySymbol> Properties => _typeSymbol.GetMembers()
            .Where(member => member.Kind == SymbolKind.Property && !member.IsStatic)
            .Select(member => member as IPropertySymbol);

        
    }
}
