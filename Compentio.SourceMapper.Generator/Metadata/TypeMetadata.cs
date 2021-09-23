using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Metadata
{
    /// <summary>
    /// Object type metadata
    /// </summary>
    interface ITypeMetadata : IMetadata
    {
        /// <summary>
        /// Type full name
        /// </summary>
        string FullName { get; }
        /// <summary>
        /// Object lis of properties
        /// </summary>
        IEnumerable<IPropertyMetadata> Properties { get; }
        /// <summary>
        /// Recurrent method that return flatten list of properties tree for the object 
        /// </summary>
        /// <param name="propertyMetadata">List of properties</param>
        /// <returns></returns>
        IEnumerable<IPropertyMetadata> FlattenProperties(IEnumerable<IPropertyMetadata> propertyMetadata);
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

        public Location? Location => _parameterSymbol.Locations.FirstOrDefault();

        public IEnumerable<IPropertyMetadata> FlattenProperties(IEnumerable<IPropertyMetadata> propertyMetadata) => 
            propertyMetadata.SelectMany(c => FlattenProperties(c.Properties)).Concat(propertyMetadata);
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

        public Location? Location => _typeSymbol.Locations.FirstOrDefault();

        public IEnumerable<IPropertyMetadata> FlattenProperties(IEnumerable<IPropertyMetadata> propertyMetadata) =>
            propertyMetadata.SelectMany(c => FlattenProperties(c.Properties)).Concat(propertyMetadata);
    }
}
