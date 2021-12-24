using Compentio.SourceMapper.Attributes;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Compentio.SourceMapper.Metadata
{
    /// <summary>
    /// Encapsulates a field that is mapped
    /// </summary>
    internal interface IFieldMetadata : IMetadata
    {
        /// <summary>
        /// Full name with namespace of the field
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Indicates whether the field is user defined class or no
        /// </summary>
        bool IsClass { get; }

        /// <summary>
        /// Indicates whether the field is static
        /// </summary>
        bool IsStatic { get; }

        /// <summary>
        /// Indicate that field should be ignored during mapping generating, due to <see cref="IgnoreMappingAttribute">
        /// </summary>
        bool IgnoreInMapping { get; }
    }

    internal class FieldMetadata : IFieldMetadata
    {
        private readonly IFieldSymbol _fieldSymbol;

        internal FieldMetadata(IFieldSymbol fieldSymbol)
        {
            _fieldSymbol = fieldSymbol;
        }

        public string Name => _fieldSymbol.Name;

        public string FullName => Type.FullName;

        public bool IsClass => _fieldSymbol.Type.SpecialType == SpecialType.None && _fieldSymbol.Type.TypeKind == TypeKind.Class;

        public bool IsStatic => _fieldSymbol.IsStatic;

        public Location? Location => _fieldSymbol.Locations.FirstOrDefault();

        private ITypeMetadata Type => new TypeMetadata(_fieldSymbol.Type);

        /// <summary>
        /// Indicate that field should be ignored during mapping generating, due to <see cref="IgnoreMappingAttribute">
        /// </summary>
        public bool IgnoreInMapping
        {
            get
            {
                var attribute = _fieldSymbol.GetAttributes().FirstOrDefault(attribute => attribute is not null && attribute.AttributeClass?.Name == nameof(IgnoreMappingAttribute));
                return attribute != null;
            }
        }
    }
}