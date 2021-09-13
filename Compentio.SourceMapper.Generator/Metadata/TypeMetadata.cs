using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Metadata
{
    interface ITypeMetadata
    {
        string Name { get; }
        string FullName { get; }
        IEnumerable<IPropertyMetadata> Properties { get; }
    }

    internal class ParameterTypeMetadata : ITypeMetadata
    {
        private readonly IParameterSymbol _parameterSymbol;

        internal ParameterTypeMetadata(IParameterSymbol parameterSymbol)
        {
            _parameterSymbol = parameterSymbol;
        }

        public string Name => _parameterSymbol.Name;
        public string FullName => _parameterSymbol.ToDisplayString();

        public IEnumerable<IPropertyMetadata> Properties => _parameterSymbol.Type.GetMembers()
            .Where(member => member as IPropertySymbol is not null)
            .Select(member => new PropertyMetadata(member as IPropertySymbol));
    }

    internal class TypeMetadata : ITypeMetadata
    {
        private readonly ITypeSymbol _typeSymbol;

        internal TypeMetadata(ITypeSymbol typeSymbol)
        {
            _typeSymbol = typeSymbol;
        }

        public string Name => _typeSymbol.Name;
        public string FullName => _typeSymbol.ToDisplayString();

        public ITypeSymbol Type => _typeSymbol;

        public IEnumerable<IPropertyMetadata> Properties => _typeSymbol.GetMembers()
            .Where(member => member.Kind == SymbolKind.Property && !member.IsStatic)
            .Select(member => new PropertyMetadata(member as IPropertySymbol));        
    }
}
