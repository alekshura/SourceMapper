using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Metadata
{
    /// <summary>
    /// Object type metadata
    /// </summary>
    internal interface ITypeMetadata : IMetadata
    {
        /// <summary>
        /// Type full name
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Object list of properties
        /// </summary>
        IEnumerable<IMemberMetadata> Properties { get; }

        /// <summary>
        /// Object list of fields <see cref="IMemberMetadata.IsField == true" />
        /// </summary>
        IEnumerable<IMemberMetadata> Fields { get; }

        /// <summary>
        /// Recurrent method that return flatten list of properties tree for the object
        /// </summary>
        /// <param name="propertyMetadata">List of properties</param>
        /// <returns></returns>
        IEnumerable<IMemberMetadata> FlattenProperties(IEnumerable<IMemberMetadata> propertyMetadata);
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

        public IEnumerable<IMemberMetadata> Properties => _parameterSymbol.Type.GetMembers()
            .Where(member => member as IPropertySymbol is not null)
            .Select(member => new MemberMetadata(member as IPropertySymbol));

        public IEnumerable<IMemberMetadata> Fields => _parameterSymbol.Type.GetMembers()
            .Where(member => member is IFieldSymbol && member.CanBeReferencedByName && !((IFieldSymbol)member).IsConst)
            .Select(member => new MemberMetadata(member as IFieldSymbol));

        public Location? Location => _parameterSymbol.Locations.FirstOrDefault();

        public IEnumerable<IMemberMetadata> FlattenProperties(IEnumerable<IMemberMetadata> propertyMetadata) =>
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

        public IEnumerable<IMemberMetadata> Properties => _typeSymbol.GetMembers()
            .Where(member => member.Kind == SymbolKind.Property && !member.IsStatic)
            .Select(member => new MemberMetadata(member as IPropertySymbol));

        public IEnumerable<IMemberMetadata> Fields => _typeSymbol.GetMembers()
            .Where(member => member.Kind == SymbolKind.Field && member.CanBeReferencedByName && !((IFieldSymbol)member).IsConst)
            .Select(member => new MemberMetadata(member as IFieldSymbol));

        public Location? Location => _typeSymbol.Locations.FirstOrDefault();

        public IEnumerable<IMemberMetadata> FlattenProperties(IEnumerable<IMemberMetadata> propertyMetadata) =>
            propertyMetadata.SelectMany(c => FlattenProperties(c.Properties)).Concat(propertyMetadata);
    }
}