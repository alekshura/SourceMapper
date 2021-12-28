using Compentio.SourceMapper.Attributes;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Metadata
{
    /// <summary>
    /// Encapsulates a member that is mapped
    /// </summary>
    internal interface IMemberMetadata : IMetadata
    {
        /// <summary>
        /// Indicate that member is field or property (or unknown)
        /// Based od <see cref="IPropertySymbol" /> and <see cref="IFieldSymbol" />
        /// </summary>
        MemberType MemberType { get; }

        /// <summary>
        /// Full name with namespace of the member
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Indicates whether the member is user defined class or no
        /// </summary>
        bool IsClass { get; }

        /// <summary>
        /// Indicates whether the member is static
        /// </summary>
        bool IsStatic { get; }

        /// <summary>
        /// Indicate that field should be ignored during mapping generating, due to <see cref="IgnoreMappingAttribute">
        /// </summary>
        bool IgnoreInMapping { get; }

        /// <summary>
        /// List of properties. It is empty in a case when the property is not user defined class: <c>IsClass == false</c> or is a field: <c>IsField == true</c>
        /// </summary>
        /// <see cref="IsClass"/>
        /// <see cref="IsField"/>
        IEnumerable<IMemberMetadata> Properties { get; }
    }

    internal class MemberMetadata : IMemberMetadata
    {
        private readonly ISymbol _symbol;

        internal MemberMetadata(ISymbol symbol)
        {
            _symbol = symbol;
        }

        public MemberType MemberType
        {
            get
            {
                if (_symbol is IFieldSymbol) return MemberType.Field;
                if (_symbol is IPropertySymbol) return MemberType.Property;

                return MemberType.Unknown;
            }
        }

        public string FullName => TypeMetadata.FullName;

        public bool IsClass => Type.SpecialType == SpecialType.None && Type.TypeKind == TypeKind.Class;

        public bool IsStatic => _symbol.IsStatic;

        public bool IgnoreInMapping
        {
            get
            {
                var attribute = _symbol.GetAttributes().FirstOrDefault(attribute => attribute is not null && attribute.AttributeClass?.Name == nameof(IgnoreMappingAttribute));
                return attribute != null;
            }
        }

        public IEnumerable<IMemberMetadata> Properties
        {
            get
            {
                if (!IsClass || MemberType != MemberType.Property)
                    return Enumerable.Empty<IMemberMetadata>();

                return Type.GetMembers()
                    .Where(member => member.Kind == SymbolKind.Property && !member.IsStatic)
                    .Select(member => new MemberMetadata(member as IPropertySymbol));
            }
        }

        public Location? Location => _symbol.Locations.FirstOrDefault();

        public string Name => _symbol.Name;

        private ITypeSymbol Type
        {
            get
            {
                if (MemberType == MemberType.Field) return ((IFieldSymbol)_symbol).Type;
                else return ((IPropertySymbol)_symbol).Type;
            }
        }

        private ITypeMetadata TypeMetadata => new TypeMetadata(Type);
    }

    /// <summary>
    /// Types of member
    /// </summary>
    internal enum MemberType
    {
        Field,
        Property,
        Unknown
    }
}