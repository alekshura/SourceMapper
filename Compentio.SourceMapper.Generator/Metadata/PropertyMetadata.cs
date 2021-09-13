using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compentio.SourceMapper.Metadata
{
    interface IPropertyMetadata
    {
        string Name { get; }
        bool IsClass { get; }
        ITypeMetadata Type { get; }
    }

    internal class PropertyMetadata : IPropertyMetadata
    {
        private readonly IPropertySymbol _propertySymbol;

        internal PropertyMetadata(IPropertySymbol propertySymbol)
        {
            _propertySymbol = propertySymbol;
        }

        public string Name => _propertySymbol.Name;

        public bool IsClass => _propertySymbol.Type.SpecialType == SpecialType.None && _propertySymbol.Type.TypeKind == TypeKind.Class;

        public ITypeMetadata Type => new TypeMetadata(_propertySymbol.Type);
    }
}
