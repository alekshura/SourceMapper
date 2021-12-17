using Compentio.SourceMapper.Attributes;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Metadata
{
    /// <summary>
    /// Encapsulates a property that is mapped
    /// </summary>
    internal interface IPropertyMetadata : IMetadata
    {
        /// <summary>
        /// Full name with namespace of the property
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Indicates whether the property is user defined class or no
        /// </summary>
        /// <see cref="Properties"/>
        bool IsClass { get; }

        /// <summary>
        /// Indicate that property should be ignored during mapping generating, due to <see cref="IgnoreMappingAttribute">
        /// </summary>
        bool IgnoreInMapping { get; }

        /// <summary>
        /// List of properties. It is empty in a case when the property is not user defined class: <c>IsClass == false</c>
        /// </summary>
        /// <see cref="IsClass"/>
        IEnumerable<IPropertyMetadata> Properties { get; }
    }

    internal class PropertyMetadata : IPropertyMetadata
    {
        private readonly IPropertySymbol _propertySymbol;

        internal PropertyMetadata(IPropertySymbol propertySymbol)
        {
            _propertySymbol = propertySymbol;
        }

        public string Name => _propertySymbol.Name;

        public string FullName => Type.FullName;

        public bool IsClass => _propertySymbol.Type.SpecialType == SpecialType.None && _propertySymbol.Type.TypeKind == TypeKind.Class;

        private ITypeMetadata Type => new TypeMetadata(_propertySymbol.Type);

        public IEnumerable<IPropertyMetadata> Properties
        {
            get
            {
                if (!IsClass)
                    return Enumerable.Empty<IPropertyMetadata>();

                return _propertySymbol.Type.GetMembers()
                    .Where(member => member.Kind == SymbolKind.Property && !member.IsStatic)
                    .Select(member => new PropertyMetadata(member as IPropertySymbol));
            }
        }

        public Location? Location => _propertySymbol.Locations.FirstOrDefault();

        /// <summary>
        /// Indicate that property should be ignored during mapping generating, due to <see cref="IgnoreMappingAttribute">
        /// </summary>
        public bool IgnoreInMapping
        {
            get
            {
                var attribute = _propertySymbol.GetAttributes().FirstOrDefault(attribute => attribute is not null && attribute.AttributeClass?.Name == nameof(IgnoreMappingAttribute));
                return attribute != null;
            }
        }
    }
}