using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Metadata;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Compentio.SourceMapper.Tests.Metadata
{
    internal class FakeMethodMetadata : MethodMetadata
    {
        internal FakeMethodMetadata(IMethodSymbol methodSymbol) : base(methodSymbol)
        {
        }

        public override IEnumerable<MappingAttribute> MappingAttributes => new List<MappingAttribute> { new MappingAttribute() };
    }
}