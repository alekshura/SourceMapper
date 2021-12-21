using AutoFixture;
using Microsoft.CodeAnalysis;
using Moq;
using System.Collections.Immutable;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public abstract class SourcesMetadataTestBase
    {
        protected readonly IFixture _fixture;
        protected readonly Mock<IAssemblySymbol> _mockAssembly;
        protected readonly ImmutableArray<IAssemblySymbol> _assemblySymbols;

        protected abstract string MockAssemblyName { get; }

        protected SourcesMetadataTestBase()
        {
            _fixture = new Fixture();
            _mockAssembly = _fixture.Build<Mock<IAssemblySymbol>>()
                .WithAutoProperties()
                .Without(n => n.DefaultValue)
                .Create();
            _assemblySymbols = ImmutableArray.Create(_mockAssembly.Object);
        }

        protected static AssemblyIdentity GetAssemblyIdentityMock(string name)
        {
            return new AssemblyIdentity(name: name);
        }
    }
}