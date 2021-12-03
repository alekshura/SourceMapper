using AutoFixture;
using AutoFixture.AutoMoq;
using Compentio.SourceMapper.Matchers;
using Compentio.SourceMapper.Metadata;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Compentio.SourceMapper.Tests.Matchers
{
    public class MethodsMatchersTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMethodMetadata> _mockMethodMetadata;
        private readonly Mock<IMapperMetadata> _mockMapperMetadata;
        private readonly Mock<IPropertyMetadata> _mockTargetPropertyMetadata;
        private readonly Mock<IPropertyMetadata> _mockSourcePropertyMetadata;
        private readonly Mock<ITypeMetadata> _mockTypeMetadata;

        public MethodsMatchersTests()
        {
            _fixture = new Fixture()
               .Customize(new AutoMoqCustomization { ConfigureMembers = true })
               .Customize(new SupportMutableValueTypesCustomization());
            _mockMethodMetadata = _fixture.Create<Mock<IMethodMetadata>>();
            _mockMapperMetadata = _fixture.Create<Mock<IMapperMetadata>>();
            _mockTargetPropertyMetadata = _fixture.Create<Mock<IPropertyMetadata>>();
            _mockSourcePropertyMetadata = _fixture.Create<Mock<IPropertyMetadata>>();
            _mockTypeMetadata = _fixture.Create<Mock<ITypeMetadata>>();
        }

        [Fact]
        public void MatchDefinedMethod_MatcherSourceAndTarget_ReturnMethod()
        {
            // Arrange
            _mockMapperMetadata.Setup(m => m.MethodsMetadata).Returns(new List<IMethodMetadata> { _mockMethodMetadata.Object });
            _mockMethodMetadata.Setup(m => m.ReturnType.FullName).Returns(_mockTargetPropertyMetadata.Object.FullName);

            _mockTypeMetadata.Setup(t => t.FullName).Returns(_mockSourcePropertyMetadata.Object.FullName);
            _mockMethodMetadata.Setup(m => m.Parameters).Returns(new List<ITypeMetadata> { _mockTypeMetadata.Object });

            // Act
            var result = _mockMapperMetadata.Object.MatchDefinedMethod(_mockSourcePropertyMetadata.Object, _mockTargetPropertyMetadata.Object);

            // Assert
            result.Should().NotBeNull();
        }
    }
}