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

        public Location? Location => _fieldSymbol.Locations.FirstOrDefault();

        private ITypeMetadata Type => new TypeMetadata(_fieldSymbol.Type);
    }
}