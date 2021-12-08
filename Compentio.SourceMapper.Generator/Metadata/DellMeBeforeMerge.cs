using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Metadata
{
    internal class DellMeBeforeMerge : IPropertyMetadata
    {
        private readonly IPropertySymbol _propertySymbol;

        internal DellMeBeforeMerge(IPropertySymbol propertySymbol)
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
    }
}