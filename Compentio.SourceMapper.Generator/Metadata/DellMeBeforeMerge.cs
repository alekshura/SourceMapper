using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Compentio.SourceMapper.Metadata
{
    internal class DellMeBeforeMerge : IPropertyMetadata
    {
        private readonly IPropertySymbol _propertySymbol;

        internal DellMeBeforeMerge(IPropertySymbol propertySymbol)
        {
            _propertySymbol = propertySymbol;
        }

        public string Name => throw new System.NotImplementedException();

        public string FullName => throw new System.NotImplementedException();

        public bool IsClass => throw new System.NotImplementedException();

        private ITypeMetadata Type => throw new System.NotImplementedException();

        public IEnumerable<IPropertyMetadata> Properties
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public Location? Location => throw new System.NotImplementedException();
    }
}